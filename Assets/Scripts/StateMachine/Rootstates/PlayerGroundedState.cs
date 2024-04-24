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
        Ctx.RequireNewJumpPress = false;
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
        //Debug.Log("Is Jump Pressed: " + Ctx.IsJumpPressed);
        //Debug.Log("New Jump Not Needed: " + !Ctx.RequireNewJumpPress);
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
                    SetSubState(Factory.Idle());
                    SwitchState(Factory.Backflip());
                }
            }
        } else if (!Ctx.IsGrounded) {
            SwitchState(Factory.Fall());
        }
    }

    public override void InitializeSubState(){
        if (!Ctx.IsCrouchPressed) {
            if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
                SetSubState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
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
