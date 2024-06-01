using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSubstate : PlayerBaseState
{
    float deltaSpeed = 4.0f;
    Vector2 deltaVelocity = Vector2.zero;
    float deltaAcceleration = 0.0f;

    public PlayerJumpSubstate(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base (currentContext,playerStateFactory) {}

    public override void EnterState(){
        // the multiplier means 1 over time to get to max speed
        deltaAcceleration = deltaSpeed*5.0f;
        deltaVelocity = Vector2.zero;
    }

    public override void UpdateState(){
        MoveAboutVelocity();
        CheckSwitchState();
    }

    public override void ExitState(){
        // This does get called, but for some reason doesn't successfully set it to zero

        // deltaVelocity = Vector2.zero;
        // deltaSpeed = 0;
        Ctx.AdditionalJumpMovementX = 0;
        Ctx.AdditionalJumpMovementZ = 0;
        // Debug.Log("jump sub exit");
    }

    public override void CheckSwitchState(){
        // if (Ctx.IsGrounded) {
        //     if (Ctx.IsMovementPressed) {
        //         if (Ctx.IsRunPressed) {
        //             SwitchState(Factory.Run());
        //         } else {
        //             SwitchState(Factory.Walk());
        //         }
        //     } else {
        //         SwitchState(Factory.Idle());
        //     }
        // }
    }

    // Experimental to cap out added velocity in air
    private void MoveAboutVelocity() {
        // Debug.Log("Target" + deltaSpeed*Ctx.CurrentMovementInput);
        // Debug.Log("Actual" + deltaVelocity);
        deltaVelocity = Vector2.MoveTowards(deltaVelocity,deltaSpeed*Ctx.CurrentMovementInput,deltaAcceleration*Ctx.TimeStep);
        Ctx.AdditionalJumpMovementX = deltaVelocity.x;
        Ctx.AdditionalJumpMovementZ = deltaVelocity.y;
    }

    public override void InitializeSubState(){}
}
