using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateAndMovementController : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    Animator animator;

    // variables to store optimized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int jumpCountHash;

    // variables to store player input
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private Vector3 appliedMovement;
    private bool isMovementPressed;
    bool isRunPressed;

    // constants
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float rotationSpeed;
    float runMult = 3.0f;
    float groundedGravity = -.05f;
    float gravity = -1.5f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = .5f;
    float maxJumpTime = 0.5f;
    bool isJumping = false;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    Dictionary<int,float> initialJumpVelocities = new Dictionary<int,float>();
    Dictionary<int,float> jumpGravities = new Dictionary<int,float>();
    Coroutine currentJumpResetRoutine = null; 

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("IsJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime/2;
        gravity = (-2 * maxJumpHeight)/Mathf.Pow(timeToApex,2);
        initialJumpVelocity = (2* maxJumpHeight)/timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight * 1.6f))/Mathf.Pow(timeToApex*1.25f,2);
        float secondInitialJumpVelocity = (2* (maxJumpHeight * 1.6f))/(timeToApex*1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight * 2.5f))/Mathf.Pow(timeToApex*1.5f,2);
        float thirdInitialJumpVelocity = (2* (maxJumpHeight * 2.5f))/(timeToApex*1.5f);
       
        initialJumpVelocities.Add(1,initialJumpVelocity);
        initialJumpVelocities.Add(2,secondInitialJumpVelocity);
        initialJumpVelocities.Add(3,thirdInitialJumpVelocity);

        jumpGravities.Add(0,gravity);
        jumpGravities.Add(1,gravity);
        jumpGravities.Add(2,secondJumpGravity);
        jumpGravities.Add(3,thirdJumpGravity);
    }

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed) {
            if (jumpCount <3 && currentJumpResetRoutine != null) {
                StopCoroutine(currentJumpResetRoutine);
            }
            animator.SetBool(isJumpingHash,true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;
            animator.SetInteger(jumpCountHash,jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];
        } else if (!isJumpPressed && isJumping && characterController.isGrounded) {
            isJumping = false;
        }
    }

    IEnumerator jumpResetRoutine() {
        yield return new WaitForSeconds(.25f);
        jumpCount = 0;
    }

    void onJump (InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onRun (InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void handleRotation()
    {
        Vector3 toLookAt = new Vector3(currentMovement.x,0.0f,currentMovement.z);
        if (toLookAt != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(toLookAt,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,rotationSpeed*Time.deltaTime);
        }
    }

    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMult;
        currentRunMovement.z = currentMovementInput.y * runMult;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        
        if (isMovementPressed && !isWalking) {
            animator.SetBool(isWalkingHash,true);
        }
        else if (!isMovementPressed && isWalking) {
            animator.SetBool(isWalkingHash,false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash,true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning) {
            animator.SetBool(isRunningHash,false);
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;
        if (characterController.isGrounded) {
            if (isJumpAnimating) {
                animator.SetBool(isJumpingHash,false);
                isJumpAnimating = false;
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                if (jumpCount == 3) {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash,jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling) {
            float previousYVel = currentMovement.y;
            currentMovement.y += (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVel+currentMovement.y)/2,-20.0f);
        }
        else {
            float previousYVel = currentMovement.y;
            currentMovement.y += (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVel+currentMovement.y)/2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        if (isRunPressed) {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;
        }
        characterController.Move(appliedMovement*Time.deltaTime*moveSpeed);
        handleGravity();
        handleJump();
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
