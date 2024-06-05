using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{

    public enum TransitionType
    {
        SameScene, DifferentScene
    }
    
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;
    private bool canTrans;

    [SerializeField] VoidEventChannel levelStartedEventChannel;

    void Update(){
        // Press Return Key, Start transition via portal
        if (canTrans) // Input.GetKeyDown(KeyCode.Return) && 
        { 
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    void OnEnable()
    {
        if (levelStartedEventChannel != null)
            levelStartedEventChannel.AddListener(action: Open);
    }

    void OnDisable()
    {
        if (levelStartedEventChannel != null)
            levelStartedEventChannel.RemoveListener(action: Open);
    }

    private void Open()
    {
        Destroy(obj: gameObject);
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag("Player")){
            canTrans = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = false;
        }
    }
}
