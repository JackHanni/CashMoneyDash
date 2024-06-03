using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLongjumpState : PlayerBaseState, IRootState
{
    float _longjumpVelocity = 11.0f;

    public PlayerLongjumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }

    public override void EnterState(){
        InitializeSubState();
        PlaySound();
        HandleJump();
    }

    private void PlaySound()
    {
        int index = Array.IndexOf(Enum.GetValues(SFX_VFX_Player.VoiceEnum.LONG_JUMP.GetType()), SFX_VFX_Player.VoiceEnum.LONG_JUMP);
        AudioSource.PlayClipAtPoint((AudioClip)Ctx.SFX_VFX_Player.voices.GetValue(index), Ctx.transform.position);
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
        Ctx.CurrentMovementX = 0;
        Ctx.CurrentMovementZ = 0;
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        Ctx.IsJumping = false;
        Ctx.IsLongjumping = false;
        Ctx.AdditionalJumpMovementX = 0;
        Ctx.AdditionalJumpMovementZ = 0;
    }

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded && Ctx.AppliedMovementY < 0.0f) {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState(){
        SetSubState(Factory.Jumpsub());
    }

    void HandleJump() {
        Ctx.Animator.SetBool(Ctx.IsSkiddingHash,true);
        Ctx.Animator.SetBool(Ctx.IsCrouchedHash,true);
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,true);
        Ctx.IsJumping = true;
        Ctx.IsLongjumping = true;
        Vector3 move = new Vector3 (0.0f,Ctx.GroundedThreshold*2.5f,0.0f);
        Ctx.CharacterController.Move(move);
        Vector2 jumpVel = new Vector2 (Ctx.AppliedMovementX,Ctx.AppliedMovementZ);
        jumpVel = jumpVel.normalized * _longjumpVelocity;
        Ctx.AppliedMovementX = jumpVel.x;
        Ctx.CurrentMovementX = jumpVel.x;
        Ctx.AppliedMovementZ = jumpVel.y;
        Ctx.CurrentMovementZ = jumpVel.y;
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[1]*0.5f;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[1]*0.5f;
    }

    public void HandleGravity() {
        float previousYVel = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY += Ctx.JumpGravities[Ctx.JumpCount] * Ctx.TimeStep * 0.4f;
        Ctx.AppliedMovementY = (previousYVel+Ctx.CurrentMovementY)*0.5f;
    }
}
