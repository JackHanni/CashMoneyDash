using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    float fallMultiplier = 1.3f;

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(.2f);
        Ctx.JumpCount = 0;
    }

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }

    public override void EnterState(){
        Ctx.RequireNewJumpPress = false;
        InitializeSubState();
        HandleJump();
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
    }

    public override void UpdateState(){
        HandleGravity();
        CheckSwitchState();
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,false);
        Ctx.IsJumping = false;
        Ctx.CurrentMovementX = 0;
        Ctx.CurrentMovementZ = 0;
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        if (Ctx.IsJumpPressed) {
            Ctx.RequireNewJumpPress = true;
        }
        else {
            Ctx.RequireNewJumpPress = false;
        }
        Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(IJumpResetRoutine());
        if (Ctx.JumpCount == 3) {
            Ctx.JumpCount = 0;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash,Ctx.JumpCount);
        }
    }

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded) {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState(){
        SetSubState(Factory.Jumpsub());
    }

    void HandleJump() {
        Vector3 move = new Vector3 (0.0f,Ctx.GroundedThreshold*2.0f,0.0f);
        Ctx.CharacterController.Move(move);
        if (Ctx.JumpCount <3 && Ctx.CurrentJumpResetRoutine != null) {
            Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
        }
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,true);
        Ctx.IsJumping = true;
        Ctx.JumpCount += 1;
        Ctx.Animator.SetInteger(Ctx.JumpCountHash,Ctx.JumpCount);
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
    }

    public void HandleGravity() {
        bool isFalling = false;//Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        if (isFalling) {
            float previousYVel = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY += (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Ctx.TimeStep);
            Ctx.AppliedMovementY = Mathf.Max((previousYVel+Ctx.CurrentMovementY)*0.5f,-20.0f);
        }
        else {
            float previousYVel = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY += (Ctx.JumpGravities[Ctx.JumpCount] * Ctx.TimeStep);
            Ctx.AppliedMovementY = (previousYVel+Ctx.CurrentMovementY)*0.5f;
        }
    }
}
