public class PlayerRunState : PlayerBaseState
{

    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsWalkingHash, true);
        Context.Animator.SetBool(Context.IsRunningHash, true);
    }

    public override void UpdateState()
    {
        Context.AppliedMovementX = Context.CurrentMovementInput.x * Context.RunMultiplier;
        Context.AppliedMovementZ = Context.CurrentMovementInput.y * Context.RunMultiplier;
        CheckSwitchStates();
    }

    public override void ExitState() {}
    public override void InitializeSubState() {}

    public override void CheckSwitchStates()
    {
        if (!Context.IsMovementPressed)
        {
            SwitchState(StateFactory.Idle());
        }
        else if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SwitchState(StateFactory.Walk());
        }
    }
}