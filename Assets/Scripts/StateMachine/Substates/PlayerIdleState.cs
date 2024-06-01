using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private Vector2 velocity;
    // 1 over the time it takes to skid to a halt
    private float acceleration = 40.0f;
    // private float initialSpeed;
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}

    public override void EnterState(){
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
        // if (Ctx.CurrentSpeed > 0) {
        //     initialSpeed = Ctx.CurrentSpeed;
        // } else {
        //     initialSpeed = 1;
        // }
        velocity.x = Ctx.AppliedMovementX + Ctx.AdditionalJumpMovementX;
        velocity.y = Ctx.AppliedMovementZ + Ctx.AdditionalJumpMovementZ;
    }

    public override void UpdateState(){
        SkidToHalt();
        CheckSwitchState();
    }

    private void SkidToHalt()
    {
        if (Ctx.AdditionalJumpMovementX != 0) {
            Ctx.AdditionalJumpMovementX = 0;
        }
        if (Ctx.AdditionalJumpMovementZ != 0) {
            Ctx.AdditionalJumpMovementZ = 0;
        }
        // if (Ctx.CurrentSpeed > 0.0f) {
        //     Ctx.CurrentSpeed -= Ctx.TimeStep*invTimeToSkid;
        // } else {
        //     Ctx.CurrentSpeed = 0.0f;
        // }
        // Ctx.AppliedMovementX *= Ctx.CurrentSpeed/initialSpeed;
        // Ctx.AppliedMovementZ *= Ctx.CurrentSpeed/initialSpeed;
        velocity = Vector2.MoveTowards(velocity,Vector2.zero,acceleration*Ctx.TimeStep);
        Ctx.AppliedMovementX = velocity.x;
        Ctx.AppliedMovementZ = velocity.y;
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
        } else {
            SwitchState(Factory.Jumpsub());
        }
    }

    public override void InitializeSubState(){}
}
