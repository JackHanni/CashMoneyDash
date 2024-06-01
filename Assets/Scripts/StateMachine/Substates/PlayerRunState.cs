using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}
     
    public override void EnterState(){
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,true);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
    }

    public override void UpdateState(){
        if (Ctx.CurrentSpeed < 1) {
            // increase the speed to max in a second. Can be faster if you multiply time step.
            Ctx.CurrentSpeed += Ctx.TimeStep;
        } else {
            Ctx.CurrentSpeed = 1;
        }
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMult * Ctx.CurrentSpeed;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMult * Ctx.CurrentSpeed;
        CheckSwitchState();
        // Debug.Log("Running");

    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
    }

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded) {
            if (Ctx.IsCrouchPressed) {
                SwitchState(Factory.Crouched());
            }
            else if (!Ctx.IsMovementPressed) {
                SwitchState(Factory.Idle());
            }
            else if (!Ctx.IsRunPressed && Ctx.IsMovementPressed) {
                SwitchState(Factory.Walk());
            }
        } else if (Ctx.IsJumping) {
            SwitchState(Factory.Jumpsub());
        }
    }

    public override void InitializeSubState(){}
}
