using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}
     
    public override void EnterState(){
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
    }

    public override void UpdateState(){
        if (Ctx.CurrentSpeed < 1) {
            // increase the speed to max in a second. Can be faster if you multiply time step.
            Ctx.CurrentSpeed += Ctx.TimeStep*2.0f;
        } else {
            Ctx.CurrentSpeed = 1;
        }
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x*Ctx.CurrentSpeed*Ctx.MoveSpeed;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y*Ctx.CurrentSpeed*Ctx.MoveSpeed;
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded) {
            if (Ctx.IsCrouchPressed) {
                SwitchState(Factory.Crouched());
            }
            else if (!Ctx.IsMovementPressed) {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsRunPressed) {
                SwitchState(Factory.Run());
            }
        } else {
            SwitchState(Factory.Jumpsub());
        }
    }

    public override void InitializeSubState(){}
}
