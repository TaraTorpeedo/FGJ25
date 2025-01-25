using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private Animator m_animator;

    private int m_isWalkingHash;
    private int m_isRunningHash;
    private int m_isJumpingHash;
    private int m_jumpCountHash;

    private PlayerInput m_playerInput;
    private Vector2 m_currentMovementInput;
    private Vector3 m_currentMovement;
    private Vector3 m_currentRunMovement;
    private Vector3 m_appliedMovement;
    private bool m_isMovementPressed;
    private bool m_isRunPressed;
    private bool m_isJumpPressed = false;
    private float m_rotationFactorPerFrame = 15.0f;
    private float m_runMultiplier = 3f;

    private float m_gravity = -9.8f;
    private float m_groundedGravity = -0.05f;

    private float m_initialJumpVelocity;
    private float m_maxJumpHeight = 2.0f;
    private float m_maxJumpTime = 0.75f;
    private bool m_isJumping = false;
    private bool m_isJumpingAnimating = false;

    private int m_jumpCount = 0;
    private Dictionary<int, float> m_initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> m_jumpGravities = new Dictionary<int, float>();

    private Coroutine m_currentJumpReset = null;

    protected void Awake()
    {
        m_playerInput =  new PlayerInput();

        m_isWalkingHash = Animator.StringToHash("isWalking");
        m_isRunningHash = Animator.StringToHash("isRunning");
        m_isJumpingHash = Animator.StringToHash("isJumping");
        m_jumpCountHash = Animator.StringToHash("jumpCount");

        m_playerInput.CharacterControls.Move.started += OnMovementInput;
        m_playerInput.CharacterControls.Move.canceled += OnMovementInput;
        m_playerInput.CharacterControls.Move.performed += OnMovementInput;

        m_playerInput.CharacterControls.Run.started += OnRun;
        m_playerInput.CharacterControls.Run.canceled += OnRun;

        m_playerInput.CharacterControls.Jump.started += OnJump;
        m_playerInput.CharacterControls.Jump.canceled += OnJump;

        SetUpJumpValues();
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
        RotateCharacter();
        AnimateCharacter();

        if (m_isRunPressed)
        {
            m_appliedMovement.x = m_currentRunMovement.x;
            m_appliedMovement.z = m_currentRunMovement.z;
        }
        else
        {
            m_appliedMovement.x = m_currentMovement.x;
            m_appliedMovement.z = m_currentMovement.z;
        }

        m_characterController.Move(m_appliedMovement * Time.deltaTime);

        CharacterGravity();
        HandelJump();
    }

    private void SetUpJumpValues()
    {
        var timeToApex = m_maxJumpTime / 2;
        m_gravity = (-2 * m_maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        m_initialJumpVelocity = (2 * m_maxJumpHeight) / timeToApex;

        var secondJumpGravity = (-2 * (m_maxJumpHeight + 2) / Mathf.Pow((timeToApex * 1.25f), 2));
        var secondJumpInitialVelocity = (2 * (m_maxJumpHeight + 2)) / (timeToApex * 1.25f);

        var thirdJumpGravity = (-2 * (m_maxJumpHeight + 4) / Mathf.Pow((timeToApex * 1.5f), 2));
        var thirdJumpInitialVelocity = (2 * (m_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        m_initialJumpVelocities.Add(1, m_initialJumpVelocity);
        m_initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        m_initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        m_jumpGravities.Add(0, m_gravity);
        m_jumpGravities.Add(1, m_gravity);
        m_jumpGravities.Add(2, secondJumpGravity);
        m_jumpGravities.Add(3, thirdJumpGravity);
    }

    private void HandelJump()
    {
        if(!m_isJumping &&  m_characterController.isGrounded && m_isJumping)
        {
            if(m_jumpCount < 3 && m_currentJumpReset != null)
            {
                StopCoroutine(ResetJump());
            }
            m_animator.SetBool(m_isJumpingHash, true);
            m_isJumping = true;
            m_isJumpingAnimating = true;
            m_jumpCount += 1;
            m_animator.SetInteger(m_jumpCountHash, m_jumpCount);
            m_currentMovement.y = m_initialJumpVelocities[m_jumpCount];
            m_appliedMovement.y = m_initialJumpVelocities[m_jumpCount];
        }
        else if(!m_isJumpPressed && m_isJumping && m_characterController.isGrounded)
        {
            m_isJumping = false;
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

    private void OnJump(InputAction.CallbackContext context)
    {
        m_isJumpPressed = context.ReadValueAsButton();
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
        var isFalling = m_currentMovement.y <= 0.0f || !m_isJumpPressed;
        var fallMultiplier = 2.0f;

        if(m_characterController.isGrounded)
        {
            if (m_isJumpingAnimating)
            {
                m_animator.SetBool(m_isJumpingHash, false);
                m_isJumpingAnimating = false;
                m_currentJumpReset = StartCoroutine(ResetJump());

                if(m_jumpCount == 3)
                {
                    m_jumpCount = 0;
                    m_animator.SetInteger(m_jumpCountHash, m_jumpCount);
                }
            }
            m_currentMovement.y = m_groundedGravity;
            m_appliedMovement.y = m_groundedGravity;
        }
        else if(isFalling)
        {
            var previousYVelocity = m_currentMovement.y;
            m_currentMovement.y = m_currentMovement.y + (m_jumpGravities[m_jumpCount] * fallMultiplier * Time.deltaTime);
            m_appliedMovement.y = Mathf.Max((previousYVelocity + m_currentMovement.y) * 0.5f, -20.0f);
        }
        else
        {
            var previousYVelocity = m_currentMovement.y;
            m_currentMovement.y = m_currentMovement.y + (m_jumpGravities[m_jumpCount] * Time.deltaTime);
            m_appliedMovement.y = (previousYVelocity + m_currentMovement.y) * 0.5f;
        }
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.5f);
        m_jumpCount = 0;
    }
}
