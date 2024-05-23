using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSubstate : PlayerBaseState
{
    float deltaSpeed = 3.0f;
    Vector2 deltaVelocity = Vector2.zero;
    float deltaAcceleration = 10.0f;

    public PlayerJumpSubstate(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}

    public override void EnterState(){
        deltaVelocity = Vector2.zero;
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
        // Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
        // Ctx.Animator.SetBool(Ctx.IsJumpingHash,true);
    }

    public override void UpdateState(){
        MoveAboutVelocity();
        CheckSwitchState();
        // Debug.Log("Jumping");
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,false);
    }

    public override void CheckSwitchState(){}

    // Experimental to cap out added velocity in air
    private void MoveAboutVelocity() {
        deltaVelocity = Vector2.MoveTowards(deltaVelocity,deltaSpeed*Ctx.CurrentMovementInput*Ctx.TimeStep,deltaAcceleration*Ctx.TimeStep);
        Ctx.AppliedMovementX += deltaVelocity.x;
        Ctx.AppliedMovementZ += deltaVelocity.y;
    }

    public override void InitializeSubState(){}
}
