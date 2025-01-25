public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleGravity();
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState(){}

    public override void CheckSwitchStates()
    {
        if(Context.IsJumpPressed && !Context.RequireNewJumpPress)
        {
            SwitchState(StateFactory.Jump());
        }
        else if(!Context.CharacterController.isGrounded)
        {
            SwitchState(StateFactory.Fall());
        }
    }

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

    public void HandleGravity()
    {
        Context.CurrentMovementY = Context.Gravity;
        Context.AppliedMovementY = Context.Gravity;
    }
}