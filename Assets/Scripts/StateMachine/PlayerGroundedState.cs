using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {
        IsRootState = true;
     }

    public override void EnterState(){
        InitializeSubState();
        HandleGravity();
    }

    public void HandleGravity() {
        Ctx.CurrentMovementY = Ctx.GroundedGravity*10;
        Ctx.AppliedMovementY = Ctx.GroundedGravity*10;
    }

    public override void UpdateState(){
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress) {
            SwitchState(Factory.Jump());
        } else if (!Ctx.IsGrounded) {
            SwitchState(Factory.Fall());
        }
    }

    public override void InitializeSubState(){
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Walk());
        }
        else {
            SetSubState(Factory.Run());
        }
    }
}
