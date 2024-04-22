using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackflipState : PlayerBaseState, IRootState
{
    private float backflipMovement = 0.9f;

    public PlayerBackflipState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }

    public override void EnterState(){
        Ctx.IsBackflipping = true;
        Vector3 forward = Ctx.CharacterController.transform.forward*backflipMovement;
        Ctx.AppliedMovementX = -forward.x;
        Ctx.AppliedMovementZ = -forward.z;
        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState(){
        HandleGravity();
        CheckSwitchState();
    }

    public override void ExitState(){
        Ctx.IsBackflipping = false;
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,false);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
        if (Ctx.IsJumpPressed) {
            Ctx.RequireNewJumpPress = true;
        }
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        
    }

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded) {
            new WaitForSeconds(.3f);
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState(){
        SetSubState(Factory.Idle());
    }

    void HandleJump() {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,true);
        Ctx.IsJumping = true;
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[3];
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[3];
    }

    public void HandleGravity() {
        float previousYVel = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY += Ctx.JumpGravities[3] * Time.deltaTime;
        Ctx.AppliedMovementY = (previousYVel+Ctx.CurrentMovementY)*0.5f;
    }
}
