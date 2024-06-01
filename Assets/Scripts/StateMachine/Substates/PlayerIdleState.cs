using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    // 1 over the time it takes to skid to a halt
    private float invTimeToSkid = 4.0f;
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}

    public override void EnterState(){
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
    }

    public override void UpdateState(){
        if (Ctx.CurrentSpeed > 0.0f) {
            Ctx.CurrentSpeed -= Ctx.TimeStep*invTimeToSkid;
        } else {
            Ctx.CurrentSpeed = 0.0f;
        }
        CheckSwitchState();
        // Debug.Log("Idling");
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded) {
            if (Ctx.IsCrouchPressed) {
                SwitchState(Factory.Crouched());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsRunPressed) {
                SwitchState(Factory.Run());
            }
            else if (Ctx.IsMovementPressed) {
                SwitchState(Factory.Walk());
            }
        } else if (Ctx.IsJumping) {
            SwitchState(Factory.Jumpsub());
        }
    }

    public override void InitializeSubState(){}
}
