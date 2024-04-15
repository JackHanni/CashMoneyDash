using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {
        IsRootState = true;
        InitializeSubState();
     }

    public override void EnterState(){
        //Debug.Log("Fall");
        Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
    }

    public override void UpdateState(){
        CheckSwitchState();
        HandleGravity();
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsFallingHash, false);
    }

    void HandleGravity() {
        float previousYVelocity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY += Ctx.Gravity*Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity+Ctx.CurrentMovementY)*0.5f,-20.0f);
    }

    public override void CheckSwitchState(){
        if (Ctx.CharacterController.isGrounded) {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState(){
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Walk());
        }
        else {
            SetSubState(Factory.Run());
        }
    }
}
