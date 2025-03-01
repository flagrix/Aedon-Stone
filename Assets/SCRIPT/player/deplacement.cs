using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public Vector3 respawnPoint;
    public bool isRespawning = false;
    public Camera playerCamera;
    public float walkSpeed = 3f;
    public float runSpeed = 8f;
    public float jumpPower = 7f;
    public float gravity = 20f;
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float crouchTransitionSpeed = 15f; // Speed of crouch height transition

    public AudioSource footstepAudioSource;  // Composant AudioSource pour les bruits de pas
    public AudioClip walkSound;              // Son pour la marche
    public AudioClip runSound;               // Son pour la course
    public float footstepIntervalWalk = 0.5f;  // Temps entre chaque bruit de pas en marchant
    public float footstepIntervalRun = 0.3f;   // Temps entre chaque bruit de pas en courant
    private float footstepTimer = 0f;          // Timer pour contr�ler les sons

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private bool isCurrentlyRunning = false; // Tracks running status
    private float targetHeight; // Target height for crouching/standing

    public float attackRange = 10f; // Port�e de l'attaque
    public int attackDamage = 40; // D�g�ts inflig�s par l'attaque
    public LayerMask enemyLayer; // Layer pour d�tecter les ennemis

    public float attackCooldown = 0.75f; // D�lai de 1 seconde entre les attaques
    private float lastAttackTime = 0f; // Temps de la derni�re attaque

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set initial height
        targetHeight = defaultHeight;
    }

    void Update()
    {
        //Get movements axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Determine if the player is grounded
        bool isGrounded = characterController.isGrounded;

        // Crouch logic (press and hold)
        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            targetHeight = crouchHeight;
        }
        else
        {
            targetHeight = defaultHeight;
        }

        // Gestion de l'attaque
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastAttackTime >= attackCooldown) // V�rifie si le cooldown est �coul�
            {
                Attack();
                lastAttackTime = Time.time; // Met � jour le temps de la derni�re attaque
            }
        }
        // Smoothly adjust height
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        // Adjust speed while crouching
        bool isCrouching = Mathf.Abs(characterController.height - crouchHeight) < 0.1f;
        if (isCrouching)
        {
            isCurrentlyRunning = false; // Cannot run while crouching
        }

        // Determine movement speed
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching)
        {
            isCurrentlyRunning = true; // Start running if grounded and shift is held
        }
        else if (isGrounded)
        {
            isCurrentlyRunning = false; // Stop running if grounded and shift is not held
        }
        //Determine in which speed the player will go
        float currentSpeed = isCrouching ? crouchSpeed : (isCurrentlyRunning ? runSpeed : walkSpeed);
        float curSpeedX = canMove ? currentSpeed * Input.GetAxis("Vertical") : 0; //Get input of W and S (return 1 or -1) 
        float curSpeedY = canMove ? currentSpeed * Input.GetAxis("Horizontal") : 0;//Get input of A and D (return 1 or -1)

        // Preserve Y-axis movement
        float movementDirectionY = moveDirection.y;

        if (isGrounded)
        {
            // Update movement direction when grounded
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }
        else
        {
            // Maintain momentum while airborne
            Vector3 horizontalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
            Vector3 inputVelocity = (forward * curSpeedX) + (right * curSpeedY);

            // Add input to current horizontal velocity
            if (inputVelocity != Vector3.zero)
            {
                horizontalVelocity = inputVelocity;
            }

            moveDirection = horizontalVelocity;
        }

        // Apply Y-axis movement
        moveDirection.y = movementDirectionY;

        // Jump logic
        if (Input.GetButton("Jump") && canMove && isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        // Apply gravity if not grounded
        if (!isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);
        PlayFootstepSound();

        // Handle camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -(lookXLimit + 10), lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

    }

    void PlayFootstepSound()
    {
        // V�rifie si le joueur touche le sol et appuie sur une touche de d�placement
        if (characterController.isGrounded && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            footstepTimer -= Time.deltaTime; // R�duit le timer

            if (footstepTimer <= 0f)  // V�rifie si on peut jouer un son
            {
                // D�termine le bon son � jouer (marche ou course)
                footstepAudioSource.clip = isCurrentlyRunning ? runSound : walkSound;

                // Ne joue le son que si l'AudioSource n'est pas d�j� en lecture
                if (!footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Play();
                }

                // R�initialise le timer selon la vitesse de d�placement
                footstepTimer = isCurrentlyRunning ? footstepIntervalRun : footstepIntervalWalk;
            }
        }
        else
        {
            // Arr�te le son si le joueur ne bouge plus ou est en l'air
            footstepAudioSource.Stop();
        }
    }
    
    public void Death()
    {
        if (instance != null && !isRespawning && !GameOver.instance.isGameOver)
        {
            Debug.Log("Le joueur est mort, lancement du respawn...");
            isRespawning = true;
            instance.enabled = false; // D�sactive le script de mouvement du joueur
            Camera.main.transform.SetParent(null); // D�tache la cam�ra du joueur
            gameObject.SetActive(false); // D�sactive l'objet joueur

            // Appeler la m�thode de respawn sur un autre objet (par exemple, un GameManager)
            GameManager.Instance.StartRespawnCoroutine(this);
            
        }
    }

    void Attack()
    {
        // Raycast pour d�tecter les ennemis devant le joueur
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Centre de l'�cran
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange, enemyLayer))
        {
            // V�rifier si l'objet touch� est un Qwertiens
            QwertiensBasic qwertiens = hit.collider.GetComponent<QwertiensBasic>();
            if (qwertiens != null)
            {
                qwertiens.SetHealth(-attackDamage);
                Debug.Log("Qwertiens touch� ! PV restants : ");
            }
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
}
