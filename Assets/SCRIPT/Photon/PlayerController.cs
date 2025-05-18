using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable, IDamageable
{
    private PhotonView photonView;
    private CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    private Vector3 networkedPosition;
    private Quaternion networkedRotation;

    public Camera playerCamera;

    [Header("Items options")]
    [SerializeField] Item[] items;
    int itemIndex;
    int prevItemIndex = -1;

    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 8f;
    public float jumpPower = 7f;
    public float gravity = 20f;

    [Header("Camera Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;

    private bool canMove = true;
    private bool isRunning = false;

    [Header("Footstep Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip walkSound;
    public AudioClip runSound;
    public float footstepIntervalWalk = 0.5f;
    public float footstepIntervalRun = 0.3f;
    private float footstepTimer = 0f;

    [Header("Attack Settings")]
    public float attackRange = 10f;
    public int attackDamage = 40;
    public LayerMask enemyLayer;
    public float attackCooldown = 0.75f;
    private float lastAttackTime = 0f;
    const float maxHealth = 100f;
    float currHealth = maxHealth;

    [Header("Animation Settings")]
    Animator Animator;

    [Header("Crouch Settings")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float crouchTransitionSpeed = 15f; // Speed of crouch height transition

    private float targetHeight; // Target height for crouching/standing
    PlayerManager playerManager;

    private HealthBar healthBar; // pour la barre de vie locale
    private Inventory inventory; //pour l'inventaire local


    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
        inventory = GetComponentInChildren<Inventory>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.SetBool("MortEnSah", false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set initial height
        targetHeight = defaultHeight;

        // D�sactiver cam�ra et audio des autres joueurs
        if (photonView.IsMine)
        {
            EquipItem(0);
            healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null)
            {
                healthBar.SetMaxHealth((int)maxHealth);
                healthBar.SetHealth((int)currHealth);
            }
        }
        else
        {
            if (playerCamera != null)
            {
                Destroy(GetComponentInChildren<Camera>().gameObject);
                AudioListener listener = playerCamera.GetComponent<AudioListener>();
                if (listener != null) listener.enabled = false;
                playerCamera.enabled = false;
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                TakeDamage(20f);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                inventory.addPotion(1);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                inventory.UsePotion();
            }
            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                    break;
                }
            }
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scroll > 0f)
            {
                int nextIndex = (itemIndex + 1) % items.Length;
                EquipItem(nextIndex);
            }
            else if (scroll < 0f)
            {
                int prevIndex = (itemIndex - 1 + items.Length) % items.Length;
                EquipItem(prevIndex);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse Down");
                items[itemIndex].Use();
            }

            if (transform.position.y < -10f) //Die if you fall out
            {
                Animator.SetBool("MortEnSah", true);
                Die();
            }

            HandleMovement();
            HandleCamera();
            HandleAttack();
            PlayFootstepSound();
            Animator.SetBool("JaiMal", false);
        }

        else
        {
            // Interpolation r�seau pour les autres joueurs
            transform.position = Vector3.Lerp(transform.position, networkedPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkedRotation, Time.deltaTime * 10f);
        }
    }
    private void HandleAttack()
    {
        Animator.SetBool("MoiJeBagarre", false);
        if (Input.GetMouseButtonDown(0)) // Clic gauche (marche pas)
        {
            
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Animator.SetBool("MoiJeBagarre", true);
                Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, attackRange, enemyLayer))
                {
                    // Ici tu touches un objet sur le bon layer
                    var enemy = hit.collider.GetComponent<QwertiensBasic>();
                    var fastEnemy = hit.collider.GetComponent<FastQwertien>();
                    var fatEnemy = hit.collider.GetComponent<FatQwertien>();

                    if (enemy != null) enemy.SetHealth(-attackDamage);
                    if (fastEnemy != null) fastEnemy.SetHealth(-attackDamage);
                    if (fatEnemy != null) fatEnemy.SetHealth(-attackDamage);
                }

                lastAttackTime = Time.time;
            }
        }
    }
    private void PlayFootstepSound()
    {
        if (!characterController.isGrounded || (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0))
        {
            footstepAudioSource.Stop();
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            footstepAudioSource.clip = Input.GetKey(KeyCode.LeftShift) ? runSound : walkSound;

            if (!footstepAudioSource.isPlaying)
                footstepAudioSource.Play();

            footstepTimer = Input.GetKey(KeyCode.LeftShift) ? footstepIntervalRun : footstepIntervalWalk;
        }
    }



    private void HandleMovement()
    {
        Animator.SetBool("SAUTE", false);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isGrounded = characterController.isGrounded;
        bool isCrouching = Mathf.Abs(characterController.height - crouchHeight) < 0.1f;
        float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        float curSpeedX = canMove ? (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        Vector3 targetDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            targetHeight = crouchHeight;
        }
        else
        {
            targetHeight = defaultHeight;
        }

        // Smoothly adjust height
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        // Adjust speed while crouching

        if (isCrouching)
        {
            isRunning = false; // Cannot run while crouching
        }

        if (isGrounded)
        {
            moveDirection = targetDirection;
            if (targetDirection != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Animator.SetBool("EnMarche", false);
                    Animator.SetBool("CoursForest", true);
                }
                else
                {
                    Animator.SetBool("EnMarche", true);
                    Animator.SetBool("CoursForest", false);
                }
            }
            else
            {
                Animator.SetBool("EnMarche", false);
                Animator.SetBool("CoursForest", false);
            }

            if (Input.GetButton("Jump") && canMove)
            {
                Animator.SetBool("EnMarche", false);
                Animator.SetBool("CoursForest", false);
                Animator.SetBool("SAUTE", true);
                moveDirection.y = jumpPower;
            }
        }
        else
        {
            moveDirection.x = targetDirection.x;
            moveDirection.z = targetDirection.z;
        }

        // Gravity
        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
    void EquipItem(int _index)
    {
        if (_index == prevItemIndex)
        {
            return;
        }
        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (prevItemIndex != -1)
        {
            items[prevItemIndex].itemGameObject.SetActive(false);
        }

        prevItemIndex = itemIndex;

        if (photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine && targetPlayer == photonView.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    private void HandleCamera()
    {
        if (!canMove) return;

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Envoi de ma position
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Réception de la position d'un autre joueur
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
    

    public void TakeDamage(float damage)
    {
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;
        Animator.SetBool("JaiMal", true);
        currHealth -= damage;

        if (healthBar != null)
            healthBar.SetActualHealth(-(int)damage); // négatif = perte de vie

        if (currHealth <= 0)
        {
            Animator.SetBool("MortEnSah", true);
            Die();
        }
    }

    public void Heal(float amount)
    {
        currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
        healthBar.SetActualHealth((int)amount);
    }

    void Die()
    {
        playerManager.Die();
    }
}
