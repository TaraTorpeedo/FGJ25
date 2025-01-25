public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsWalkingHash, true);
        Context.Animator.SetBool(Context.IsRunningHash, false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Context.AppliedMovementX = Context.CurrentMovementInput.x;
        Context.AppliedMovementZ = Context.CurrentMovementInput.y;
    }

    public override void ExitState() {}

    public override void InitializeSubState() {}
    
    public override void CheckSwitchStates()
    {
        if (!Context.IsMovementPressed)
        {
            SwitchState(StateFactory.Idle());
        }
        else if (Context.IsMovementPressed && Context.IsRunPressed)
        {
            SwitchState(StateFactory.Run());
        }
    }
}
