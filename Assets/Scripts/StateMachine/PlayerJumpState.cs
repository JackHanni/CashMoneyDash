using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{

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
        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState(){
        HandleGravity();
        CheckSwitchState();
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,false);
        if (Ctx.IsJumpPressed) {
            Ctx.RequireNewJumpPress = true;
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

    public override void InitializeSubState(){}

    void HandleJump() {
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
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;
        if (isFalling) {
            float previousYVel = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY += (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVel+Ctx.CurrentMovementY)*0.5f,-20.0f);
        }
        else {
            float previousYVel = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY += (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVel+Ctx.CurrentMovementY)*0.5f;
        }
    }
}
