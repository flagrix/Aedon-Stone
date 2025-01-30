using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
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
    private float footstepTimer = 0f;          // Timer pour contrôler les sons

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private bool isCurrentlyRunning = false; // Tracks running status
    private float targetHeight; // Target height for crouching/standing

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
        rotationX = Mathf.Clamp(rotationX, -(lookXLimit+10), lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
        
    }

    void PlayFootstepSound()
    {
        // Vérifie si le joueur touche le sol et appuie sur une touche de déplacement
        if (characterController.isGrounded && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            footstepTimer -= Time.deltaTime; // Réduit le timer

            if (footstepTimer <= 0f)  // Vérifie si on peut jouer un son
            {
                // Détermine le bon son à jouer (marche ou course)
                footstepAudioSource.clip = isCurrentlyRunning ? runSound : walkSound;

                // Ne joue le son que si l'AudioSource n'est pas déjà en lecture
                if (!footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Play();
                }

                // Réinitialise le timer selon la vitesse de déplacement
                footstepTimer = isCurrentlyRunning ? footstepIntervalRun : footstepIntervalWalk;
            }
        }
        else
        {
            // Arrête le son si le joueur ne bouge plus ou est en l'air
            footstepAudioSource.Stop();
        }
    }
}
