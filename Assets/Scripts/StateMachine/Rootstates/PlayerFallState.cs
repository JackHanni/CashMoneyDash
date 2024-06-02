using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {
        IsRootState = true;
     }

    public override void EnterState(){
        InitializeSubState();
        Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
        PlaySound();
    }

    private void PlaySound()
    {
        int index = Array.IndexOf(Enum.GetValues(SFXPlayer.SoundEnum.LONG_JUMP.GetType()), SFXPlayer.SoundEnum.LONG_JUMP);
        AudioSource.PlayClipAtPoint((AudioClip)Ctx.SFXPlayer.sounds.GetValue(index), Ctx.transform.position);
    }

    public override void UpdateState(){
        HandleGravity();
        CheckSwitchState();
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsFallingHash, false);
    }

    public void HandleGravity() {
        float previousYVelocity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY += Ctx.Gravity*Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity+Ctx.CurrentMovementY)*0.5f,-20.0f);
    }

    public override void CheckSwitchState(){
        if (Ctx.IsGrounded) {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState(){
        SetSubState(Factory.Jumpsub());
    }
}
