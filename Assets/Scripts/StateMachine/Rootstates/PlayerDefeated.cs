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
        PlaySound();


        Ctx.playerDefeatedEventChannel.Broadcast();
        // lock cursor

        // rotate character to face camera

        // zoom in

        Ctx.Animator.Play("Defeated");
    }

    private void PlaySound()
    {
        AudioClip death = Ctx.SFX_VFX_Player.lose_sounds[UnityEngine.Random.Range(0, Ctx.SFX_VFX_Player.lose_sounds.Length)];
        AudioSource.PlayClipAtPoint(death, Ctx.transform.position);
    }

    public override void ExitState()
    {
        throw new NotImplementedException();
    }

    public void HandleGravity()
    {
        throw new NotImplementedException();
    }

    public override void InitializeSubState()
    {
        throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }

    

}
