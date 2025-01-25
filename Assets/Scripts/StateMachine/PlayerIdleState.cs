public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsWalkingHash, false);
        Context.Animator.SetBool(Context.IsRunningHash, false);

        Context.AppliedMovementX = 0f;
        Context.AppliedMovementZ = 0f;
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState(){}
    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (Context.IsMovementPressed && Context.IsRunPressed)
        {
            SwitchState(StateFactory.Run());
        }
        else if (Context.IsMovementPressed)
        {
            SwitchState(StateFactory.Walk());
        }
    }

}
