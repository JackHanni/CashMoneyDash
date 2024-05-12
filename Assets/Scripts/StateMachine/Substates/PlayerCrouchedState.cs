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
        //Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        //Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
    }

    public override void UpdateState(){
        if (Ctx.IsGrounded) {
            Ctx.AppliedMovementX *= Ctx.SkidMultiplier;
            Ctx.AppliedMovementZ *= Ctx.SkidMultiplier;
        }
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (!Ctx.IsCrouchPressed) {
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
    }

    public override void InitializeSubState(){}
}
