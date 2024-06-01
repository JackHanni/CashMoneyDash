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
        Ctx.CurrentMovementY = Ctx.GroundedGravity;
        Ctx.AppliedMovementY = Ctx.GroundedGravity;
    }

    public override void UpdateState(){
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress) {
            if (!Ctx.IsCrouchPressed) {
                SwitchState(Factory.Jump());
            } else {
                Vector2 velocity = new Vector2 (Ctx.AppliedMovementX,Ctx.AppliedMovementZ);
                // if going fast enough, long jump
                if (velocity.sqrMagnitude > Ctx.LongJumpThreshold) {
                    SwitchState(Factory.Longjump());
                } else {
                    // else backflip
                    SwitchState(Factory.Backflip());
                }
            }
        } else if (!Ctx.IsGrounded) {
            SwitchState(Factory.Fall());
        }
    }

    public override void InitializeSubState(){
        // SetSubState(Factory.Idle());
        
        if (!Ctx.IsCrouchPressed) {
           if (!Ctx.IsMovementPressed) {
               SetSubState(Factory.Idle());
           }
           else if (!Ctx.IsRunPressed) {
               SetSubState(Factory.Walk());
           }
           else {
               SetSubState(Factory.Run());
           }
        } else {
           SetSubState(Factory.Crouched());
        }
        
    }
}
