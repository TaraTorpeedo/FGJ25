using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if(Context.CharacterController.isGrounded)
        {
            SwitchState(StateFactory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Context.Animator.SetBool(Context.IsFallingHash, true);
    }

    public override void ExitState() => Context.Animator.SetBool(Context.IsFallingHash, false);

    public override void InitializeSubState()
    {
        if (!Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(StateFactory.Idle());
        }
        else if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(StateFactory.Walk());
        }
        else
        {
            SetSubState(StateFactory.Run());
        }
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public void HandleGravity()
    {
        var previousYVelocity = Context.CurrentMovementY;
        Context.CurrentMovementY = Context.CurrentMovementY + Context.Gravity * Time.deltaTime;
        Context.AppliedMovementY = Mathf.Max((previousYVelocity + Context.CurrentMovementY) * 0.5f, -20.0f);
    }
}
