using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackflipState : PlayerBaseState, IRootState
{
    private float backflipMovement = 2.0f;
    private float backflipVelocity;

    public PlayerBackflipState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }

    public override void EnterState(){
        Ctx.IsBackflipping = true;
        InitializeSubState();
        backflipVelocity = (Ctx.InitialJumpVelocities[3]+Ctx.InitialJumpVelocities[2])*.5f;
        HandleJump();
    }

    public override void UpdateState(){
        HandleGravity();
        CheckSwitchState();
        // Currently can't move while in backflip
        // Ctx.CurrentMovementInput = Vector2.zero;
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,false);
        Ctx.IsJumping = false;
        Ctx.IsBackflipping = false;
        if (Ctx.IsJumpPressed) {
            Ctx.RequireNewJumpPress = true;
        }
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        Ctx.CurrentMovementX = 0;
        Ctx.CurrentMovementZ = 0;
        Ctx.AdditionalJumpMovementX = 0;
        Ctx.AdditionalJumpMovementZ = 0;
    }

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded && Ctx.AppliedMovementY < 0.0f) {
            // new WaitForSeconds(.3f);
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState(){
        SetSubState(Factory.Jumpsub());
    }

    void HandleJump() {
        Vector3 move = new Vector3 (0.0f,Ctx.GroundedThreshold*1.5f,0.0f);
        Ctx.CharacterController.Move(move);

        Vector3 forward = Ctx.CharacterController.transform.forward;
        Vector2 backflipDirection = new Vector2 (-forward.x,-forward.z);
        backflipDirection = backflipDirection.normalized*backflipMovement;
        // Ctx.CurrentMovementX = backflipDirection.x;
        Ctx.AppliedMovementX = backflipDirection.x;
        // Ctx.CurrentMovementZ = backflipDirection.y;
        Ctx.AppliedMovementZ = backflipDirection.y;

        Ctx.Animator.SetBool(Ctx.IsJumpingHash,true);
        Ctx.IsJumping = true;

        Ctx.CurrentMovementY = backflipVelocity;
        Ctx.AppliedMovementY = backflipVelocity;
    }

    public void HandleGravity() {
        float previousYVel = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY += Ctx.JumpGravities[2] * Ctx.TimeStep;
        Ctx.AppliedMovementY = (previousYVel+Ctx.CurrentMovementY)*0.5f;
    }
}
