using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSubstate : PlayerBaseState
{
    float deltaSpeed = 5.0f;
    Vector2 deltaVelocity = Vector2.zero;
    float deltaAcceleration = 1.0f;

    public PlayerJumpSubstate(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}

    public override void EnterState(){
        // deltaVelocity = Vector2.zero;
        // Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        // Ctx.Animator.SetBool(Ctx.IsRunningHash,false);
        // Ctx.Animator.SetBool(Ctx.IsCrouchedHash,false);
    }

    public override void UpdateState(){
        // MoveAboutVelocity();
        Ctx.AppliedMovementX += deltaAcceleration*Ctx.CurrentMovementInput.x*Ctx.TimeStep;
        Ctx.AppliedMovementZ += deltaAcceleration*Ctx.CurrentMovementInput.y*Ctx.TimeStep;
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){}

    // Experimental to cap out added velocity in air
    private void MoveAboutVelocity() {
        Vector3 camRelMovement3D = Ctx.CameraTransform.InverseTransformVector(Ctx.CurrentMovementInput.x,0.0f,Ctx.CurrentMovementInput.y);
        Vector2 camRelMovement2D = new Vector2(camRelMovement3D.x,camRelMovement3D.z);
        deltaVelocity = Vector2.MoveTowards(deltaVelocity, deltaSpeed*camRelMovement2D, Ctx.TimeStep*deltaAcceleration);
        Ctx.CharacterController.Move(deltaVelocity*Ctx.TimeStep);
    }

    public override void InitializeSubState(){}
}
