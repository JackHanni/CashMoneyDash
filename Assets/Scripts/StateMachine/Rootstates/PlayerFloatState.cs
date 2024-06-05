//using UnityEngine;

//public class PlayerFloatState : PlayerBaseState, IRootState
//{ 
//    float floatingSpeed = 200f; 
//    Vector3 floatingPositionOffset = new Vector3(0f, 5f, -4f);
//    Transform playerTransform;

//    Vector3 floatingPosition;

//    public PlayerFloatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
//    : base(currentContext, playerStateFactory)
//    {
//        IsRootState = true;
//    }

//    public override void CheckSwitchState()
//    {
//        throw new System.NotImplementedException();
//    }


//    public override void EnterState()
//    {
//        Ctx.playerDefeatedEventChannel.Broadcast();
//        playerTransform = Ctx.transform;

//        floatingPosition = playerTransform.position + floatingPositionOffset;
//        Debug.Log("float state - "+ playerTransform.position.ToString() +"," + floatingPosition.ToString());
//    }

//    public override void ExitState()
//    {
//        throw new System.NotImplementedException();
//    }

//    public void HandleGravity()
//    {
//        if (Ctx.IsGrounded)
//            Ctx.AppliedMovementY = Ctx.GroundedGravity;
//        else
//            Ctx.AppliedMovementY += Ctx.GroundedGravity * Ctx.TimeStep;
//    }

//    public override void InitializeSubState()
//    {
//        throw new System.NotImplementedException();
//    }


//    public override void UpdateState()
//    {
        

//        if (Vector3.Distance(playerTransform.position, floatingPosition) > floatingSpeed *  Time.deltaTime)
//        {
//            playerTransform.position = Vector3.MoveTowards(playerTransform.position, floatingPosition, floatingSpeed * Time.deltaTime);
//            Debug.Log("if here - "+ playerTransform.position + ","+floatingSpeed + ","+Time.deltaTime);
//        }
//        else
//        {
//            Debug.Log("else here");
//            floatingPosition += new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
//        }

//        HandleGravity();
//        Ctx.CurrentMovementX = 0;
//        Ctx.CurrentMovementZ = 0;
//        Ctx.AppliedMovementX = 0;
//        Ctx.AppliedMovementZ = 0;
//        Ctx.AdditionalJumpMovementX = 0;
//        Ctx.AdditionalJumpMovementZ = 0;
//    }
//}