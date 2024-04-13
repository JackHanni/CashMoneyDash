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
    }

    public override void UpdateState(){
        CheckSwitchState();
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMult;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMult;
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (!Ctx.IsMovementPressed) {
            SwitchState(Factory.Idle());
        }
        else if (!Ctx.IsRunPressed) {
            SwitchState(Factory.Walk());
        }
    }

    public override void InitializeSubState(){}
}
