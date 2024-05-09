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
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y;
        CheckSwitchState();
        // Debug.Log("Walk");
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (!Ctx.IsCrouchPressed) {
            if (!Ctx.IsMovementPressed) {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsRunPressed) {
                SwitchState(Factory.Run());
            }
        } else {
            SwitchState(Factory.Crouched());
        }
    }

    public override void InitializeSubState(){}
}
