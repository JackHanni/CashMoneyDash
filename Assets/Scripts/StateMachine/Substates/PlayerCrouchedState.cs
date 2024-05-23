using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchedState : PlayerBaseState
{
    public PlayerCrouchedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}

    public override void EnterState(){
        // start crouch animation
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,true);
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
    }

    public override void UpdateState(){
        if (Ctx.IsGrounded) {
            Ctx.AppliedMovementX *= Ctx.SkidMultiplier;
            Ctx.AppliedMovementZ *= Ctx.SkidMultiplier;
        }
        CheckSwitchState();
        // Debug.Log("Crouching");

    }

    public override void ExitState(){
    }

    public override void CheckSwitchState(){
        if (!Ctx.IsCrouchPressed && Ctx.IsGrounded) {
            if (!Ctx.IsMovementPressed) {
                SwitchState(Factory.Idle());
            }
            else if (!Ctx.IsRunPressed) {
                SwitchState(Factory.Walk());
            } 
            else {
                SwitchState(Factory.Run());
            }
        }
        //else if (Ctx.IsJumping) {
        //    Debug.Log("crouched update called for backflip");
        //    SwitchState(Factory.Jumpsub());
        //}
    }

    public override void InitializeSubState(){}
}
