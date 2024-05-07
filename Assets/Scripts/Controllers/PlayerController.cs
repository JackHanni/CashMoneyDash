using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private CharacterStats characterStats;

    void Awake(){
        agent = GetComponent<NavMeshAgent>();
        characterStats = GetComponent<CharacterStats>();
    }

    void Start(){
        // register method in event
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        characterStats.MaxHealth = 2; // edit values
    }   

    public void MoveToTarget(Vector3 target){
        // register this method to mouse manager events, invoke when click on ground 
        agent.destination = target;
    }
}
