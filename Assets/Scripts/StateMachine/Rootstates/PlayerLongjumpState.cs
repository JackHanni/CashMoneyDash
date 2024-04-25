using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLongjumpState : PlayerBaseState, IRootState
{
    float _longjumpVelocity = 4.0f;

    public PlayerLongjumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }

    public override void EnterState(){
        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState(){
        HandleGravity();
        CheckSwitchState();
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,false);
        Ctx.Animator.SetBool(Ctx.IsSkiddingHash,false);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
        if (Ctx.IsJumpPressed) {
            Ctx.RequireNewJumpPress = true;
        }
    }

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded) {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState(){}

    void HandleJump() {
        Ctx.Animator.SetBool(Ctx.IsSkiddingHash,true);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,true);
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,true);
        Ctx.IsJumping = true;
        Vector3 move = new Vector3 (0.0f,Ctx.GroundedThreshold*1.5f,0.0f);
        Ctx.CharacterController.Move(move);
        Vector2 jumpVel = new Vector2 (Ctx.AppliedMovementX,Ctx.AppliedMovementZ);
        jumpVel = jumpVel.normalized * _longjumpVelocity;
        Ctx.AppliedMovementX = jumpVel.x;
        Ctx.AppliedMovementZ = jumpVel.y;
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[1] * 0.8f;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[1] * 0.8f;
    }

    public void HandleGravity() {
        float previousYVel = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY += (Ctx.JumpGravities[Ctx.JumpCount] * Ctx.TimeStep);
        Ctx.AppliedMovementY = (previousYVel+Ctx.CurrentMovementY)*0.5f;
    }
}
