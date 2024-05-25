using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    Transform player;
    float attackRange = 1.0f;
    float stunTimespan = 0.8f;
    float stunTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // This is fixed time animation and time in state.
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        DamagePlayer(animator);
        stunTime = 0.0f;
        // animator.SetBool("isAttacking", false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);
        agent.Warp(animator.transform.position + 0.01f*Vector3.Normalize(animator.transform.position - player.position));
        // float distance = Vector3.Distance(player.position,animator.transform.position);
        // if (distance > attackRange) {
        //     animator.SetBool("isAttacking", false);
        // }
        stunTime += Time.deltaTime;
        if (stunTime >= stunTimespan) {
            animator.SetBool("isAttacking", false);
        }

    }

    public void DamagePlayer(Animator animator) {
        var playerScript = player.GetComponent<PlayerStateMachine>();
        playerScript.DamagePlayer(1,agent.transform);
        // Transition atomatically to chasing again
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
