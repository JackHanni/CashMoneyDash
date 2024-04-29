using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

// what is a singleton?  
// Generally speaking, a singleton in Unity is a globally accessible class that exists in the scene, but only once.
public class SceneController : MonoBehaviour //Singleton<SceneController> 
{
    
    GameObject player;

    public void TransitionToDestination(TransitionPoint transitionPoint){
        // two possible conditions - same scene or different scene

        // switch(transitionPoint.transitionType)
        // {
        //     case TransitionPoint.TransitionType.SameScene:
        //         // current scene, go to portal at destination tag
        //         StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
        //         break;
        //     case TransitionPoint.TransitionType.DifferentScene:
        //         break;
        // }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        // put player to the portal at destinationTag
        // player = GameManager.Instance.playerStats.gameObject;
        // player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
        // yield return null;
        yield return null;
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        // find the destination with the same tag from all destinations
        // var entrances = FindObjectsOfType<TransitionDestination>();
        //
        // for(int i = 0; i < entrances.Length; i++)
        // {
        //     if (entrances[i].destinationTag == destinationTag)
        //         return entrances[i];
        // }
        // return null;
        return null;
    }
}
