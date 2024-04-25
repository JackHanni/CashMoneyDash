using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}

    public override void EnterState(){
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
    }

    public override void UpdateState(){
        //Ctx.AppliedMovement = Vector3.MoveTowards(Ctx.AppliedMovement,Vector3.zero,2.0f*Ctx.TimeStep);
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (!Ctx.IsCrouchPressed) {
            if (Ctx.IsMovementPressed && Ctx.IsRunPressed) {
                SwitchState(Factory.Run());
            }
            else if (Ctx.IsMovementPressed) {
                SwitchState(Factory.Walk());
            }
        } else {
            SwitchState(Factory.Crouched());
        }
        
    }

    public override void InitializeSubState(){}
}
