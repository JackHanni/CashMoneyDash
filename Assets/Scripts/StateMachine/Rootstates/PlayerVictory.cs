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

        // rotate character to face camera
        FaceCamera();

        // slows down timer
        SlowDownTimer();

        Ctx.Animator.Play("Victory");
    }

    private void SlowDownTimer()
    {
        // Set the time scale to slow down the game
        // For example, setting it to 0.5 will make the game run at half speed
        Time.timeScale = 0.5f;
    }

    private void FaceCamera()
    {
        Camera mainCamera = Camera.main;

        // Calculate the direction from the character to the camera
        Vector3 directionToCamera = mainCamera.transform.position - Ctx.transform.position;

        // Remove the y component to keep the character upright
        directionToCamera.y = 0;

        // Calculate the new rotation
        Quaternion newRotation = Quaternion.LookRotation(directionToCamera);

        // Apply the new rotation
        Ctx.transform.rotation = newRotation;
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
        Time.timeScale = 1.0f;
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
