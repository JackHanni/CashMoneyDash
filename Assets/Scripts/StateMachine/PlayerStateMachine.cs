using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    Animator _animator;

    // variables to store optimized setter/getter parameter IDs
    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isJumpingHash;
    private int _jumpCountHash;
    private int _isFallingHash;
    private int _isCrouchedHash;
    private int _isSkiddingHash;

    // variables to store player input
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _currentRunMovement;
    private Vector3 _appliedMovement;
    private Vector3 _cameraRelativeMovement;
    private bool _isMovementPressed;
    private bool _isRunPressed;

    // constants
    [SerializeField]
    protected float _moveSpeed;
    [SerializeField]
    protected float _rotationSpeed;
    float _runMult = 3.0f;
    float _groundedGravity = -1.0f;
    float _gravity = -5.25f;
    public float _groundedThreshold;
    private float _timeStep = 0.02f;

    // jumping variables
    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = .5f;
    float _maxJumpTime = 0.8f;
    bool _isJumping = false;
    bool _requireNewJumpPress = false;
    int _jumpCount = 0;
    Dictionary<int,float> _initialJumpVelocities = new Dictionary<int,float>();
    Dictionary<int,float> _jumpGravities = new Dictionary<int,float>();
    Coroutine _currentJumpResetRoutine = null;
    private bool _isGrounded;
    private int _groundLayer;
    private bool _isOnWall;
    private int _wallLayer;

    private bool _isBackflipping = false;

    // crouch variables
    private float _skidMultiplier = 0.96f;
    private float _longJumpThreshold = 0.2f;
    private bool _isCrouchPressed = false;

    // State Variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // Getters and Setters
    public CharacterController CharacterController {get {return _characterController;}}
    public PlayerBaseState CurrentState { get {return _currentState; } set {_currentState = value;}}
    public Animator Animator { get {return _animator;}}
    public Coroutine CurrentJumpResetRoutine { get { return _currentJumpResetRoutine;} set {_currentJumpResetRoutine = value;}}
    public Dictionary<int,float> InitialJumpVelocities { get {return _initialJumpVelocities;}}
    public Dictionary<int,float> JumpGravities { get {return _jumpGravities;} set {_jumpGravities = value;}}
    public int JumpCount { get { return _jumpCount;} set { _jumpCount = value;}}
    public int IsJumpingHash {get{return _isJumpingHash;}}
    public int IsWalkingHash {get { return _isWalkingHash;}}
    public int IsRunningHash {get { return _isRunningHash;}}
    public int JumpCountHash {get{return _jumpCountHash;}}
    public int IsFallingHash { get { return _isFallingHash;}}
    public int IsCrouchedHash { get { return _isCrouchedHash;}}
    public int IsSkiddingHash { get { return _isSkiddingHash;}}
    public bool RequireNewJumpPress {get {return _requireNewJumpPress;} set { _requireNewJumpPress=value;}}
    public bool IsJumping {set {_isJumping = value;}}
    public bool IsJumpPressed {get { return _isJumpPressed; }}
    public bool IsCrouchPressed { get {return _isCrouchPressed;}}
    public float CurrentMovementY {get { return _currentMovement.y;} set {_currentMovement.y = value;}}
    public float AppliedMovementY { get { return _appliedMovement.y;} set {_appliedMovement.y = value;}}
    public float GroundedGravity {get {return _groundedGravity;}}
    public bool IsMovementPressed {get {return _isMovementPressed;}}
    public bool IsRunPressed {get {return _isRunPressed;}}
    public float AppliedMovementX {get {return _appliedMovement.x;} set {_appliedMovement.x = value;}}
    public float AppliedMovementZ {get {return _appliedMovement.z;} set {_appliedMovement.z = value;}}
    public float CurrentMovementX {get {return _currentMovement.x;} set {_currentMovement.x = value;}}
    public float CurrentMovementZ {get {return _currentMovement.z;} set {_currentMovement.z = value;}}
    public float CameraRelativeMovementX { get {return _cameraRelativeMovement.x;} set {_cameraRelativeMovement.x = value;}}
    public float CameraRelativeMovementZ { get {return _cameraRelativeMovement.z;} set {_cameraRelativeMovement.z = value;}}
    public float RunMult { get { return _runMult;}}
    public Vector2 CurrentMovementInput { get { return _currentMovementInput;}}
    public float Gravity { get { return _gravity;}}
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsOnWall { get { return _isOnWall; } }
    public float SkidMultiplier { get { return _skidMultiplier;}}
    public float LongJumpThreshold { get { return _longJumpThreshold;}}
    public float TimeStep { get { return _timeStep;}}
    public float RotationSpeed { get { return _rotationSpeed;}}
    public bool IsBackflipping {get {return _isBackflipping;} set {_isBackflipping = value;}}
    public float GroundedThreshold { get { return _groundedThreshold;}}
    public Vector3 AppliedMovement { get { return _appliedMovement;} set { _appliedMovement = value;}}
    public Transform CameraTransform {get { return Camera.main.transform;}}

    // Variables used locally for determining grounded logic
    private float _radius;
    private Vector3 _offset;
    private RaycastHit _hit;

    void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _radius = CharacterController.radius;
        _offset = new Vector3(0.0f,_radius,0.0f);

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        _isGrounded = false;

        _groundLayer = LayerMask.NameToLayer("Ground");
        _wallLayer = LayerMask.NameToLayer("Wall");

        _isWalkingHash = Animator.StringToHash("IsWalking");
        _isRunningHash = Animator.StringToHash("IsRunning");
        _isJumpingHash = Animator.StringToHash("IsJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");
        _isFallingHash = Animator.StringToHash("isFalling");
        _isCrouchedHash = Animator.StringToHash("isCrouched");
        _isSkiddingHash = Animator.StringToHash("isSkidding");

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;
        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;
        _playerInput.CharacterControls.Crouch.started += OnCrouch;
        _playerInput.CharacterControls.Crouch.canceled += OnCrouch;

        SetupJumpVariables();
    }

    void SetupJumpVariables()
    {
        float timeToApex = _maxJumpTime/2;
        _gravity = (-2 * _maxJumpHeight)/Mathf.Pow(timeToApex,2);
        _initialJumpVelocity = (2* _maxJumpHeight)/timeToApex;
        float secondJumpGravity = (-2 * (_maxJumpHeight * 1.6f))/Mathf.Pow(timeToApex*1.25f,2);
        float secondInitialJumpVelocity = (2* (_maxJumpHeight * 1.6f))/(timeToApex*1.25f);
        float thirdJumpGravity = (-2 * (_maxJumpHeight * 2.5f))/Mathf.Pow(timeToApex*1.5f,2);
        float thirdInitialJumpVelocity = (2* (_maxJumpHeight * 2.5f))/(timeToApex*1.5f);
       
        _initialJumpVelocities.Add(1,_initialJumpVelocity);
        _initialJumpVelocities.Add(2,secondInitialJumpVelocity);
        _initialJumpVelocities.Add(3,thirdInitialJumpVelocity);

        _jumpGravities.Add(0,_gravity);
        _jumpGravities.Add(1,_gravity);
        _jumpGravities.Add(2,secondJumpGravity);
        _jumpGravities.Add(3,thirdJumpGravity);
    }

    // Start is called before the first frame update
    // void Start(){
    //     _characterController.Move(_appliedMovement*Time.deltaTime);
    // }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(_isGrounded);
        if (!_isBackflipping) {
            HandleRotation();
        }
    }

    void FixedUpdate() {
        _currentState.UpdateStates();
        _isGrounded = checkIfGrounded();
        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        if (!_isBackflipping) {
            _characterController.Move(_cameraRelativeMovement*_moveSpeed*_timeStep);  
        } else {
            _characterController.Move(_appliedMovement*_moveSpeed*_timeStep);
        }
    }

    public void HandleRotation()
    {
        Vector3 toLookAt = new Vector3(_cameraRelativeMovement.x,0.0f,_cameraRelativeMovement.z);
        if (toLookAt != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(toLookAt,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,_rotationSpeed*Time.deltaTime);
        }
    }

    void OnCrouch (InputAction.CallbackContext context) {
        _isCrouchPressed = context.ReadValueAsButton();
    }

    void OnJump (InputAction.CallbackContext context)
    {
        _requireNewJumpPress = false;
        _isJumpPressed = context.ReadValueAsButton();
    }

    void OnRun (InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    void OnMovementInput (InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _currentRunMovement.x = _currentMovementInput.x * _runMult;
        _currentRunMovement.z = _currentMovementInput.y * _runMult;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Slime") {
            other.GetComponent<Slime>().TakeDamage(1);
        }
    }
    
 // These aren't working, but they don't hurt anything.
    // public void OnTriggerEnter(Collider collision) {
    //     Debug.Log(collision.gameObject.layer);
    //     if (collision.gameObject.layer == _groundLayer) {
    //         _isGrounded = true;
    //     } else if (collision.gameObject.layer == _wallLayer) {
    //         _isOnWall = true;
    //     }
    // }

    // public void OnTriggerExit(Collider collision) {
    //     Debug.Log("Exit Collision");
    //     if (collision.gameObject.layer == _groundLayer) {
    //         _isGrounded = false;
    //     } else if (collision.gameObject.layer == _wallLayer) {
    //         _isOnWall = false;
    //     }
    // }

/*
    public void OnCollisionStay(Collision collision) {
        Debug.Log("Colliding");
        if (!_isGrounded) {
            if (collision.gameObject.layer == _groundLayer) {
                _isGrounded = true;
            }
        }
        if (!_isOnWall) {
            if (collision.gameObject.layer == _wallLayer) {
                _isOnWall = true;
            }
        }
    }
    */

    // Using a raycast to check if we're grounded. Might be inefficient.
    private bool checkIfGrounded() {
        return Physics.SphereCast(transform.position+_offset, _radius, Vector3.down, out _hit, _groundedThreshold, ~_groundLayer);
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate) {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Vector3 rotatedVector = vectorToRotate.x*cameraRight + vectorToRotate.z*cameraForward;
        rotatedVector.y = vectorToRotate.y;
        return rotatedVector;
    }
    
}
