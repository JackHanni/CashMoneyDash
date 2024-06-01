using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent _agent;
    Transform _player;
    float _chaseRange = 10.0f;
    float _attackRange = 0.84f;
    Animator _animator;
    float _distance;
    Vector3 _separationVector;
    float _skidThreshold;
    bool _isSkidding = false;
    Vector3 _skidVector;
    Vector3 _lateralSeparationVector;
    float _lateralDistance;
    float _skidTimer;
    float _skidTime = 1.0f; // skid for this amount of time

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator;
        _agent = animator.GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent.speed = 9.0f;

        _distance = _chaseRange;
        _lateralDistance = _distance;
        _skidThreshold = _attackRange*.7f;
        _skidTimer = 0;
        _separationVector = _player.position - animator.transform.position;
        _skidVector = _separationVector.normalized;
        _lateralSeparationVector.x = _separationVector.x;
        _lateralSeparationVector.y = 0.0f;
        _lateralSeparationVector.z = _separationVector.z;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckSwitchState(animator);
        DetermineIfSkidding();
        if (_isSkidding){
            Skid();
        } else {
            PursuePlayer();
        }
    }

    private void DetermineIfSkidding()
    {
        _lateralSeparationVector.x = _separationVector.x;
        _lateralSeparationVector.z = _separationVector.z;
        _lateralDistance = _lateralSeparationVector.magnitude;
        if (!_isSkidding) {
            if (_lateralDistance < _skidThreshold){
                // begin the skid
                _isSkidding = true;
                _skidVector = _player.position + 5*_lateralSeparationVector.normalized;
            }
        } else {
            _skidTimer += Time.deltaTime;
            if (_skidTimer > _skidTime){
                // if I've skidded through long enough, then stop skidding and reset the timer
                _isSkidding = false;
                _skidTimer = 0;
            }
        }
    }

    private void Skid()
    {
        _agent.SetDestination(_skidVector);
    }

    private void CheckSwitchState(Animator animator)
    {
        _separationVector = _player.position - animator.transform.position;
        _distance = _separationVector.magnitude;
        if (_distance > _chaseRange*1.2f) {
            animator.SetBool("isChasing",false);
        }
        if (_distance < _attackRange) {
            animator.SetBool("isAttacking", true);
        }
    }

    private void PursuePlayer()
    {
        // go towards player
        _agent.SetDestination(_player.position);
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(animator.transform.position);
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
