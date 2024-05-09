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
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (!Ctx.IsCrouchPressed || Ctx.IsJumping) {
            if (Ctx.IsMovementPressed && Ctx.IsRunPressed) {
                Debug.Log("Switch to Run");
                SwitchState(Factory.Run());
            }
            else if (Ctx.IsMovementPressed) {
                Debug.Log("Switch to Walk");

                SwitchState(Factory.Walk());
            }
        } else {
            Debug.Log("Switch to Crouched");

            SwitchState(Factory.Crouched());
        }
        
    }

    public override void InitializeSubState(){}
}
