using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private Animator m_animator;

    private int m_isWalkingHash;
    private int m_isRunningHash;
    private int m_isJumpingHash;
    private int m_jumpCountHash;
    private int m_isFallingHash;

    private PlayerInput m_playerInput;
    private Vector2 m_currentMovementInput;
    private Vector3 m_currentMovement;
    private Vector3 m_appliedMovement;
    private Vector3 m_cameraRelativeMovement;

    private bool m_isMovementPressed;
    private bool m_isRunPressed;
    private float m_rotationFactorPerFrame = 15.0f;
    private float m_runMultiplier = 3f;

    private float m_gravity = -9.8f;

    private float m_initialJumpVelocity;
    private float m_maxJumpHeight = 2.0f;
    private float m_maxJumpTime = 0.75f;

    private bool m_isJumpPressed = false;
    private bool m_isJumping = false;
    private bool m_requireNewJumpPress = false;

    private int m_jumpCount = 0;
    private Dictionary<int, float> m_initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> m_jumpGravities = new Dictionary<int, float>();

    private Coroutine m_currentJumpCoroutine = null;

    private PlayerBaseState m_currentState;
    private PlayerStateFactory m_stateFactory;

    public PlayerBaseState CurrentState { get { return m_currentState;} set { m_currentState = value; } }
    public Animator Animator => m_animator;
    public CharacterController CharacterController => m_characterController;
    public Coroutine CurrentJumpCoroutine { get { return m_currentJumpCoroutine; } set { m_currentJumpCoroutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities => m_initialJumpVelocities;
    public Dictionary<int, float> JumpGravities => m_jumpGravities;

    public Vector2 CurrentMovementInput => m_currentMovementInput;

    public bool IsJumpPressed => m_isJumpPressed;
    public bool IsJumping {set { m_isJumping = value; } }
    public bool RequireNewJumpPress { get { return m_requireNewJumpPress; } set { m_requireNewJumpPress = value; } }
    public bool IsMovementPressed => m_isMovementPressed;
    public bool IsRunPressed => m_isRunPressed;

    public int JumpCount { get { return m_jumpCount; } set { m_jumpCount = value; } }
    public int JumpCountHash => m_jumpCountHash;
    public int IsJumpingHash => m_isJumpingHash;
    public int IsWalkingHash => m_isWalkingHash;
    public int IsRunningHash => m_isRunningHash;
    public int IsFallingHash => m_isFallingHash;

    public float CurrentMovementY { get { return m_currentMovement.y; } set { m_currentMovement.y = value; } }
    public float AppliedMovementY { get { return m_appliedMovement.y; } set { m_appliedMovement.y = value; } }
    public float AppliedMovementZ { get { return m_appliedMovement.z; } set { m_appliedMovement.z = value; } }
    public float AppliedMovementX { get { return m_appliedMovement.x; } set { m_appliedMovement.x = value; } }
    public float RunMultiplier => m_runMultiplier;
    public float Gravity => m_gravity;

    protected void Awake()
    {
        m_playerInput =  new PlayerInput();

        m_stateFactory = new PlayerStateFactory(this);
        m_currentState = m_stateFactory.Grounded();
        m_currentState.EnterState();

        m_isWalkingHash = Animator.StringToHash("isWalking");
        m_isRunningHash = Animator.StringToHash("isRunning");
        m_isJumpingHash = Animator.StringToHash("isJumping");
        m_jumpCountHash = Animator.StringToHash("jumpCount");
        m_isFallingHash = Animator.StringToHash("isFalling");

        m_playerInput.CharacterControls.Move.started += OnMovementInput;
        m_playerInput.CharacterControls.Move.canceled += OnMovementInput;
        m_playerInput.CharacterControls.Move.performed += OnMovementInput;

        m_playerInput.CharacterControls.Run.started += OnRun;
        m_playerInput.CharacterControls.Run.canceled += OnRun;

        m_playerInput.CharacterControls.Jump.started += OnJump;
        m_playerInput.CharacterControls.Jump.canceled += OnJump;

        SetUpJumpValues();
    }

    protected void Start() => m_characterController.Move(m_appliedMovement * Time.deltaTime);

    protected void OnEnable()
    {
        Cursor.visible = false;
        m_playerInput.CharacterControls.Enable();
    }

    protected void OnDisable() => m_playerInput.CharacterControls.Disable();

    protected void Update()
    {
        RotateCharacter();
        m_currentState.UpdateStates();

        m_cameraRelativeMovement = ConvertToCameraSpace(m_appliedMovement);
        m_characterController.Move(m_cameraRelativeMovement * Time.deltaTime);
    }

    private void RotateCharacter()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = m_cameraRelativeMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = m_cameraRelativeMovement.z;
        
        var currentRotation = transform.rotation;

        if (m_isMovementPressed)
        {
            if (positionToLookAt.sqrMagnitude > .01f)
            {
                var targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, m_rotationFactorPerFrame * Time.deltaTime);
            }
        }
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        var currentYValue = vectorToRotate.y;

        var camForward = Camera.main.transform.forward;
        var camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        var camForwardZProduct = vectorToRotate.z * camForward;
        var camRightXProduct = vectorToRotate.x * camRight;

        var vectorToRotatedToCamSpace = camForwardZProduct + camRightXProduct;
        vectorToRotatedToCamSpace.y = currentYValue;
        return vectorToRotatedToCamSpace;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        m_currentMovementInput = context.ReadValue<Vector2>();
        m_isMovementPressed = m_currentMovementInput.x != 0 || m_currentMovementInput.y != 0;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        m_isRunPressed = context.ReadValueAsButton();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        m_isJumpPressed = context.ReadValueAsButton();
        m_requireNewJumpPress = false;
    }

    private void SetUpJumpValues()
    {
        var timeToApex = m_maxJumpTime / 2;
        var initialGravity = (-2 * m_maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        m_initialJumpVelocity = (2 * m_maxJumpHeight) / timeToApex;

        var secondJumpGravity = (-2 * (m_maxJumpHeight + 2) / Mathf.Pow((timeToApex * 1.25f), 2));
        var secondJumpInitialVelocity = (2 * (m_maxJumpHeight + 2)) / (timeToApex * 1.25f);

        var thirdJumpGravity = (-2 * (m_maxJumpHeight + 4) / Mathf.Pow((timeToApex * 1.5f), 2));
        var thirdJumpInitialVelocity = (2 * (m_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        m_initialJumpVelocities.Add(1, m_initialJumpVelocity);
        m_initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        m_initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        m_jumpGravities.Add(0, initialGravity);
        m_jumpGravities.Add(1, initialGravity);
        m_jumpGravities.Add(2, secondJumpGravity);
        m_jumpGravities.Add(3, thirdJumpGravity);
    }
}