using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private int timesInGroundState;
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {
        IsRootState = true;
        InitializeSubState();
     }

    public override void EnterState(){
        timesInGroundState = 0;
        Ctx.CurrentMovementY = Ctx.GroundedGravity;
        Ctx.AppliedMovementY = Ctx.GroundedGravity;
    }

    public override void UpdateState(){
        timesInGroundState += 1;
        Ctx.CurrentMovementY = Ctx.GroundedGravity;
        Ctx.AppliedMovementY = Ctx.GroundedGravity;
        Debug.Log(timesInGroundState);
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress) {
            SwitchState(Factory.Jump());
        } else if (!Ctx.CharacterController.isGrounded) {
            //Debug.Log("Poo");
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
