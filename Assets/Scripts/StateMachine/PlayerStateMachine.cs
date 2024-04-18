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
    int _isWalkingHash;
    int _isRunningHash;
    int _isJumpingHash;
    int _jumpCountHash;
    int _isFallingHash;

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
    float _groundedGravity = -.1f;
    float _gravity = -1.5f;

    // jumping variables
    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = .5f;
    float _maxJumpTime = 0.5f;
    bool _isJumping = false;
    bool _requireNewJumpPress = false;
    int _jumpCount = 0;
    Dictionary<int,float> _initialJumpVelocities = new Dictionary<int,float>();
    Dictionary<int,float> _jumpGravities = new Dictionary<int,float>();
    Coroutine _currentJumpResetRoutine = null;
    [SerializeField]
    private bool _isGrounded;
    private int _groundLayer;
    private bool _isOnWall;
    private int _wallLayer;

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
    public bool RequireNewJumpPress {get {return _requireNewJumpPress;} set { _requireNewJumpPress=value;}}
    public bool IsJumping {set {_isJumping = value;}}
    public bool IsJumpPressed {get { return _isJumpPressed; }}
    public float CurrentMovementY {get { return _currentMovement.y;} set {_currentMovement.y = value;}}
    public float AppliedMovementY { get { return _appliedMovement.y;} set {_appliedMovement.y = value;}}
    public float GroundedGravity {get {return _groundedGravity;}}
    public bool IsMovementPressed {get {return _isMovementPressed;}}
    public bool IsRunPressed {get {return _isRunPressed;}}
    public float AppliedMovementX {get {return _appliedMovement.x;} set {_appliedMovement.x = value;}}
    public float AppliedMovementZ {get {return _appliedMovement.z;} set {_appliedMovement.z = value;}}
    public float RunMult { get { return _runMult;}}
    public Vector2 CurrentMovementInput { get { return _currentMovementInput;}}
    public float Gravity { get { return _gravity;}}
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsOnWall { get { return _isOnWall; } }
    
    // Variables used locally
    private float _radius;
    private Vector3 _offset;
    private RaycastHit hit;

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

        _isGrounded = true;

        _groundLayer = LayerMask.NameToLayer("Ground");
        _wallLayer = LayerMask.NameToLayer("Wall");

        _isWalkingHash = Animator.StringToHash("IsWalking");
        _isRunningHash = Animator.StringToHash("IsRunning");
        _isJumpingHash = Animator.StringToHash("IsJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");
        _isFallingHash = Animator.StringToHash("isFalling");

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;
        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;

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
    void Start(){
        _characterController.Move(_appliedMovement*Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = checkIfGrounded();
        HandleRotation();
        _currentState.UpdateStates();
        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        _characterController.Move(_cameraRelativeMovement*Time.deltaTime*_moveSpeed);
        
    }

    void HandleRotation()
    {
        Vector3 toLookAt = new Vector3(_cameraRelativeMovement.x,0.0f,_cameraRelativeMovement.z);
        if (toLookAt != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(toLookAt,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,_rotationSpeed*Time.deltaTime);
        }
    }

     void OnJump (InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
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

    /*
 // These aren't working, but they don't hurt anything.
    public void OnTriggerEnter(Collider collision) {
        Debug.Log("Start Collide");
        if (collision.gameObject.layer == _groundLayer) {
            _isGrounded = true;
        } else if (collision.gameObject.layer == _wallLayer) {
            _isOnWall = true;
        }
    }

    public void OnTriggerExit(Collider collision) {
        Debug.Log("Exit Collision");
        if (collision.gameObject.layer == _groundLayer) {
            _isGrounded = false;
        } else if (collision.gameObject.layer == _wallLayer) {
            _isOnWall = false;
        }
    }
*/
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
        return Physics.SphereCast(transform.position+_offset, _radius, Vector3.down, out hit, 0.1f, ~_groundLayer);
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate) {
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
