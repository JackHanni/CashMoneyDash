using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    private Vector2 velocity;
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}
     
    public override void EnterState(){
        velocity.x = Ctx.AppliedMovementX + Ctx.AdditionalJumpMovementX;
        velocity.y = Ctx.AppliedMovementZ + Ctx.AdditionalJumpMovementZ;
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,true);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
    }

    public override void UpdateState(){
        HandleMovement();
        CheckSwitchState();
    }

    private void HandleMovement() 
    {
        if (Ctx.AdditionalJumpMovementX != 0) {
            Ctx.AdditionalJumpMovementX = 0;
        }
        if (Ctx.AdditionalJumpMovementZ != 0) {
            Ctx.AdditionalJumpMovementZ = 0;
        }
        // if (Ctx.CurrentSpeed < Ctx.RunMult) {
        //     // increase the speed to max in a second. Can be faster if you multiply time step.
        //     Ctx.CurrentSpeed += Ctx.TimeStep*2.0f;
        // } else {
        //     Ctx.CurrentSpeed = Ctx.RunMult;
        // }
        // Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.CurrentSpeed*Ctx.MoveSpeed;
        // Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.CurrentSpeed*Ctx.MoveSpeed;
        velocity = Vector2.MoveTowards(velocity,Ctx.RunMult*Ctx.MoveSpeed*Ctx.CurrentMovementInput,25.0f*Ctx.TimeStep);
        Ctx.AppliedMovementX = velocity.x;
        Ctx.AppliedMovementZ = velocity.y;
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
        } else {
            SwitchState(Factory.Jumpsub());
        }
    }

    public override void InitializeSubState(){}
}
