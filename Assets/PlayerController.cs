using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private Animator m_animator;

    private int m_isWalkingHash;
    private int m_isRunningHash;

    private PlayerInput m_playerInput;
    private Vector2 m_currentMovementInput;
    private Vector3 m_currentMovement;
    private Vector3 m_currentRunMovement;
    private bool m_isMovementPressed;
    private bool m_isRunPressed;
    private float m_rotationFactorPerFrame = 15.0f;
    private float m_runMultiplier = 3f;

    protected void Awake()
    {
        m_playerInput =  new PlayerInput();

        m_isWalkingHash = Animator.StringToHash("isWalking");
        m_isRunningHash = Animator.StringToHash("isRunning");

        m_playerInput.CharacterControls.Move.started += OnMovementInput;
        m_playerInput.CharacterControls.Move.canceled += OnMovementInput;
        m_playerInput.CharacterControls.Move.performed += OnMovementInput;

        m_playerInput.CharacterControls.Run.started += OnRun;
        m_playerInput.CharacterControls.Run.canceled += OnRun;
    }

    protected void OnEnable()
    {
        m_playerInput.CharacterControls.Enable();
    }

    protected void OnDisable()
    {
        m_playerInput.CharacterControls.Disable();
    }

    protected void Update()
    {
        CharacterGravity();
        RotateCharacter();
        AnimateCharacter();
        if (m_isRunPressed)
        {
            m_characterController.Move(m_currentRunMovement * Time.deltaTime);
        }
        else
        {
            m_characterController.Move(m_currentMovement * Time.deltaTime);
        }
    }

    private void AnimateCharacter()
    {
        var isWalking = m_animator.GetBool(m_isWalkingHash);
        var isRunning = m_animator.GetBool(m_isRunningHash);

        if(m_isMovementPressed && !isWalking)
        {
            m_animator.SetBool(m_isWalkingHash, true);
        }
        else if(!m_isMovementPressed && isWalking)
        {
            m_animator.SetBool(m_isWalkingHash, false);
        }

        if((m_isMovementPressed && m_isRunPressed) && !isRunning)
        {
            m_animator.SetBool(m_isRunningHash, true);
        }
        else if((!m_isMovementPressed || !m_isRunPressed) && isRunning)
        {
            m_animator.SetBool(m_isRunningHash, false);
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        m_currentMovementInput = context.ReadValue<Vector2>();
        m_currentMovement.x = m_currentMovementInput.x;
        m_currentMovement.z = m_currentMovementInput.y;
        m_currentRunMovement.x = m_currentMovementInput.x * m_runMultiplier;
        m_currentRunMovement.z = m_currentMovementInput.y * m_runMultiplier;
        m_isMovementPressed = m_currentMovementInput.x != 0 || m_currentMovementInput.y != 0;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        m_isRunPressed = context.ReadValueAsButton();
    }

    private void RotateCharacter()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = m_currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = m_currentMovement.z;

        var currentRotation = transform.rotation;


        if(m_isMovementPressed)
        {
            var targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, m_rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void CharacterGravity()
    {
        if(m_characterController.isGrounded)
        {
            var groundedGravity = -0.05f;
            m_currentMovement.y = groundedGravity;
            m_currentRunMovement.y = groundedGravity;
        }
        else
        {
            var gravity = -9.8f;
            m_currentMovement.y += gravity;
            m_currentRunMovement.y += gravity;
        }
    }
}
