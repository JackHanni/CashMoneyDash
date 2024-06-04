using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerDefeated : PlayerBaseState, IRootState
{

    public PlayerDefeated(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory): base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        //SwitchState(Factory.Float());
    }

    public override void EnterState(){
        //PlaySound();


        // rotate character to face camera
        //FaceCamera();

        // slows down timer
        SlowDownTimer();

        Ctx.Animator.Play("Defeated");

        Ctx.playerDefeatedEventChannel.Broadcast();

    }


    private void SlowDownTimer()
    {
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
        AudioClip death = Ctx.SFX_VFX_Player.lose_sounds[UnityEngine.Random.Range(0, Ctx.SFX_VFX_Player.lose_sounds.Length)];
        AudioSource.PlayClipAtPoint(death, Ctx.transform.position);
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
    }


}
