using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable, IDamageable
{
    private PhotonView photonView;
    private CharacterController characterController;
    public PlayerItemInventory playerItemInventory;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    private Vector3 networkedPosition;
    private Quaternion networkedRotation;

    public Camera playerCamera;

    private bool isGameOverState = false;
   
    [Header("Health Bar Settings")]
    [SerializeField]public Slider healthBar;
    //[SerializeField]public int actual_health = 100;
    [Header("Damage Flash Settings")]
    public Image damageImage; // Image rouge qui recouvre l'�cran
    public float flashDuration = 0.7f; // Dur�e du flash en secondes
    private float flashTimer;

    [Header("Movement Settings")]
    public static float walkSpeed = 15f;
    public static float runSpeed = 21f;
    public static float jumpPower = 7f;
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
    public LayerMask enemyLayer;
    public static float maxHealth = 100f;
    public static float currHealth = 100f;

    [Header("Animation Settings")]
    Animator Animator;

    [Header("Crouch Settings")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float crouchTransitionSpeed = 15f; // Speed of crouch height transition

    private float targetHeight; // Target height for crouching/standing
    PlayerManager playerManager;
    
    private Inventory inventory; //pour l'inventaire local
    private Dictionary<itemType, string> stringprefab = new Dictionary<itemType, string>() { } ;
    public GameObject playerHUD;

    public bool EnPause = false;
    public GameObject EscapeMenu;
    public GameObject GameOver;

    void Awake()
    {
        {
            EscapeMenu.SetActive(false);
            photonView = GetComponent<PhotonView>();
            characterController = GetComponent<CharacterController>();

            // Sécuriser l'accès à InstantiationData et PlayerManager
            if (photonView != null && photonView.InstantiationData != null && photonView.InstantiationData.Length > 0)
            {
                PhotonView managerView = PhotonView.Find((int)photonView.InstantiationData[0]);
                if (managerView != null)
                {
                    playerManager = managerView.GetComponent<PlayerManager>();
                }
                else
                {
                    Debug.LogWarning("PlayerManager PhotonView has already been destroyed.");
                }
            }
            else
            {
                Debug.LogWarning("InstantiationData is null or empty.");
            }

            inventory = GetComponentInChildren<Inventory>();
        }

        //  healthBar = GetComponentInChildren<HealthBar>();
        
        stringprefab.Add(itemType.Arbalete, "PhotonPrefabs/arbalete");
        stringprefab.Add(itemType.FlameBook, "PhotonPrefabs/flamobook");
        stringprefab.Add(itemType.Hallebarde, "PhotonPrefabs/hallebarde (1)");
        stringprefab.Add(itemType.Axe, "PhotonPrefabs/MetallAx2 (1)");
        stringprefab.Add(itemType.PharmacoBook, "PhotonPrefabs/PharmacoBook (1)");
        stringprefab.Add(itemType.LongSword, "PhotonPrefabs/LongSword (1)");
        stringprefab.Add(itemType.Hammer, "PhotonPrefabs/uploads_files_3650488_Blacksmith's+Rounding+Hammerno+sharps");
    }

    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.SetBool("MortEnSah", false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!photonView.IsMine && playerHUD != null)
        {
            playerHUD.SetActive(false);
        }
        // Set initial height
        targetHeight = defaultHeight;

        // D�sactiver cam�ra et audio des autres joueurs
        if (photonView.IsMine)
        {
            ///EquipItem(0);
           /* healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null)
            {
                SetMaxHealth((int)maxHealth);
                SetHealth((int)currHealth);
            }*/
            
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
            if (!EnPause && !GameOver.activeInHierarchy) 
            {
                if (transform.position.y < -10f)
                {
                    Animator.SetBool("MortEnSah", true);
                    Die();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EscapeMenu.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    EnPause = true;
                }

                SetMaxHealth();
                HandleMovement();
                HandleCamera();
                PlayFootstepSound();
                Animator.SetBool("JaiMal", false);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape)&& !GameOver.activeInHierarchy)
                {
                    EscapeMenu.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    EnPause = false;
                }
            }
            
        }

        else
        {
            // Interpolation r�seau pour les autres joueurs
            transform.position = Vector3.Lerp(transform.position, networkedPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkedRotation, Time.deltaTime * 10f);
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

    public void SetMaxHealth()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currHealth;
    }

    public bool CanAfford(float amount)
    {
        return Inventory_global.runes >= amount;
    }
   

    public void SetHealth()
    {
        if (TowerHealth.instance.health <= 0) return;
        if (healthBar != null)
            healthBar.value = currHealth;

        if (currHealth <= 0)
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }

    public void SetActualHealth(int delta)
    {
        if (TowerHealth.instance.health <= 0) return;
        if (delta < 0 && currHealth > 0)
        {
            TriggerDamageFlash();
        }

        currHealth = Mathf.Clamp(currHealth + delta, 0, maxHealth);
        Debug.Log("it's me "+delta + " health is " + currHealth);
        SetHealth();
    }

  
    private void TriggerDamageFlash()
    {
        flashTimer = flashDuration;
        damageImage.color = new Color(1f, 0f, 0f, 1f);
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

        
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        

        if (isCrouching)
        {
            isRunning = false; 
            Animator.SetBool("EnMarche", true);
            Animator.SetBool("CoursForest", false);
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
    

    private void HandleCamera()
    {
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

    public void SetGameOverState(bool isOver)
    {
        if (photonView.IsMine)
        {
            isGameOverState = isOver;
            canMove = !isOver;  // bloque la possibilité de bouger quand game over
        }

        if (isOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;
        Animator.SetBool("JaiMal", true);
        
        SetActualHealth(-(int)damage); // négatif = perte de vie
        if (currHealth <= 0)
        {
            Animator.SetBool("MortEnSah", true);
            Die();
        }
    }
    public void Heal(float amount)
    {
        SetActualHealth((int)amount);
    }

    public void SetActive(bool active)
    {
        if (playerCamera != null)
        {
            playerCamera.enabled = active;

            AudioListener listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = active;
        }

        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            source.enabled = active;
        }
        enabled = active;
        characterController.enabled = active;

        if (playerCamera != null)
        {
            playerCamera.enabled = active;
            playerCamera.GetComponent<AudioListener>().enabled = active;
        }
    }
    void Die()
    {
        if (photonView.IsMine)
        {
            // Désactive les AudioSource du joueur
            foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
            {
                source.Stop();
                source.enabled = false;
            }

            // Désactive l'AudioListener (l'ouïe du joueur)
            AudioListener listener = GetComponentInChildren<AudioListener>();
            if (listener != null)
            {
                listener.enabled = false;
            }
            PlayerItemInventory playerInventory = GetComponent<PlayerItemInventory>();
            foreach (var VARIABLE in playerInventory.inventoryList)
            {
                if (VARIABLE != itemType.Hammer)
                {
                    PhotonNetwork.Instantiate(stringprefab[VARIABLE], position: playerInventory.throwItem_GameObject.transform.position,
                        Quaternion.Euler(
                            Random.Range(75f, 105f),
                            Random.Range(0f, 360f),
                            Random.Range(-15f, 15f)
                        ));
                    Debug.Log(VARIABLE);
                }
            }
        }
        Debug.Log("dead");
        //SetActive(false);
        PhotonNetwork.Destroy(gameObject);
        playerManager.StartRespawn();
    }

}
