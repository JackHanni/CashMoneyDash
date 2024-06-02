using System;
using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    float fallMultiplier = 1.3f;
    //[SerializeField] AudioClip jumpSFX;
    //[SerializeField] AudioClip[] complexJumpSFX;

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
        PlaySound();
        HandleJump();
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
    }

    private void PlaySound()
    {
        if (Ctx.JumpCount < 1)
        {
            int index = Array.IndexOf(Enum.GetValues(SFXPlayer.SoundEnum.SINGLE_JUMP.GetType()), SFXPlayer.SoundEnum.SINGLE_JUMP); 
            AudioSource.PlayClipAtPoint((AudioClip)Ctx.SFXPlayer.sounds.GetValue(index), Ctx.transform.position);
        }
        else if (Ctx.JumpCount == 1)
        {
            int index = Array.IndexOf(Enum.GetValues(SFXPlayer.SoundEnum.DOUBLE_JUMP.GetType()), SFXPlayer.SoundEnum.DOUBLE_JUMP);
            index = UnityEngine.Random.Range(index, index+2);
            AudioSource.PlayClipAtPoint((AudioClip)Ctx.SFXPlayer.sounds.GetValue(index), Ctx.transform.position);
        }
        else
        {
            int index = Array.IndexOf(Enum.GetValues(SFXPlayer.SoundEnum.TRIPLE_JUMP.GetType()), SFXPlayer.SoundEnum.TRIPLE_JUMP);
            index = UnityEngine.Random.Range(index, index + 2);
            AudioSource.PlayClipAtPoint((AudioClip)Ctx.SFXPlayer.sounds.GetValue(index), Ctx.transform.position);
        }

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
        // Ctx.AppliedMovementX = 0;
        // Ctx.AppliedMovementZ = 0;
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
        // Ctx.AdditionalJumpMovementX = 0;
        // Ctx.AdditionalJumpMovementZ = 0;
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
