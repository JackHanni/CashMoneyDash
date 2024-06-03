using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerVictory : PlayerBaseState, IRootState
{

    public PlayerVictory(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        PlaySound();

        // lock cursor

        // rotate character to face camera

        // zoom in

        Ctx.Animator.Play("Victory");
    }

    private void PlaySound()
    {
        AudioClip win = Ctx.SFX_VFX_Player.win_sounds[UnityEngine.Random.Range(0, Ctx.SFX_VFX_Player.win_sounds.Length)];
        AudioSource.PlayClipAtPoint(win, Ctx.transform.position);
    }

    public override void CheckSwitchState()
    {
        throw new NotImplementedException();
    }


    public override void ExitState()
    {
        throw new NotImplementedException();
    }

    public void HandleGravity()
    {
        if(Ctx.IsGrounded)
            Ctx.AppliedMovementY = Ctx.GroundedGravity;
        else
            Ctx.AppliedMovementY += Ctx.GroundedGravity*Ctx.TimeStep;
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
    }


}
