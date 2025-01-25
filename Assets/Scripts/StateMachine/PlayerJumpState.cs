using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.5f);
        Context.JumpCount = 0;
    }

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsJumpingHash, false);
        if (Context.IsJumpPressed)
        {
            Context.RequireNewJumpPress = true;
        }

        Context.CurrentJumpCoroutine = Context.StartCoroutine(ResetJump());

        if (Context.JumpCount == 3)
        {
            Context.JumpCount = 0;
            Context.Animator.SetInteger(Context.JumpCountHash, Context.JumpCount);
        }
    }

    public override void CheckSwitchStates()
    {
        if(Context.CharacterController.isGrounded)
        {
            SwitchState(StateFactory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        if(!Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(StateFactory.Idle());
        }
        else if(Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(StateFactory.Walk());
        }
        else
        {
            SetSubState(StateFactory.Run());
        }

    }

    public void HandleJump()
    {
        if (Context.JumpCount < 3 && Context.CurrentJumpCoroutine != null)
        {
            Context.StopCoroutine(Context.CurrentJumpCoroutine);
        }
        Context.Animator.SetBool(Context.IsJumpingHash, true);
        Context.IsJumping = true;
        Context.JumpCount += 1;
        Context.Animator.SetInteger(Context.JumpCountHash, Context.JumpCount);
        Context.CurrentMovementY = Context.InitialJumpVelocities[Context.JumpCount];
        Context.AppliedMovementY = Context.InitialJumpVelocities[Context.JumpCount];
    }

    public void HandleGravity()
    {
        var isFalling = Context.CurrentMovementY <= 0.0f || !Context.IsJumpPressed;
        var fallMultiplier = 2.0f;

        if (isFalling)
        {
            var previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY = Context.CurrentMovementY + (Context.JumpGravities[Context.JumpCount] * fallMultiplier * Time.deltaTime);
            Context.AppliedMovementY = Mathf.Max((previousYVelocity + Context.CurrentMovementY) * 0.5f, -20.0f);
        }
        else
        {
            var previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY = Context.CurrentMovementY + (Context.JumpGravities[Context.JumpCount] * Time.deltaTime);
            Context.AppliedMovementY = (previousYVelocity + Context.CurrentMovementY) * 0.5f;
        }
    }
}
