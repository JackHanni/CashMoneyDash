using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerDefeated : PlayerBaseState, IRootState
{
    Animator animator;

    public PlayerDefeated(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory): base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
    }

    public override void EnterState(){
        Ctx.playerDefeatedEventChannel.Broadcast();

        PlaySound();

        // slows down timer
        SlowDownTimer();

        animator = Ctx.Animator;
        animator.Play("Defeated");
    }

    private void PlaySound()
    {
        AudioClip lose = Ctx.SFX_VFX_Player.lose_sounds[UnityEngine.Random.Range(0, Ctx.SFX_VFX_Player.lose_sounds.Length)];
        AudioSource.PlayClipAtPoint(lose, Ctx.transform.position);
    }

    private void SlowDownTimer()
    {
        Time.timeScale = 0.5f;
    }


    public override void ExitState()
    {
        Time.timeScale = 1.0f;
    }

    public void HandleGravity()
    {
        if (Ctx.IsGrounded)
            Ctx.AppliedMovementY = Ctx.GroundedGravity;
        else
            Ctx.AppliedMovementY += Ctx.GroundedGravity * Ctx.TimeStep;
    }

    public override void InitializeSubState()
    {
        throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        HandleGravity();
        Ctx.CurrentMovementX = 0;
        Ctx.CurrentMovementZ = 0;
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        Ctx.AdditionalJumpMovementX = 0;
        Ctx.AdditionalJumpMovementZ = 0;

        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //bool IsAnimationFinished = stateInfo.IsName("Defeated") && stateInfo.normalizedTime >= 1.0f;

        //if (IsAnimationFinished)
        //{
        //    Debug.Log("switch state");
        //    SwitchState(Factory.Float());
            
        //}
    }


}
