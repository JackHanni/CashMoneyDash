using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

// what is a singleton?  
// Generally speaking, a singleton in Unity is a globally accessible class that exists in the scene, but only once.
public class SceneController : Singleton<SceneController> 
{
    public GameObject playerPrefab; // player prefab for loading and applying existing data when transfer to new scene
    GameObject player; 
    Transform playerAgent; // current player in the scene


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TransitionToDestination(TransitionPoint transitionPoint){
        // two possible conditions - same scene or different scene

        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                // current scene, go to portal at the destination tag
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                // async load the future level
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }


    // async load 
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        // TODO: SAVE DATA

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            // Diff scene: load scene
            yield return SceneManager.LoadSceneAsync(sceneName);
            // yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            yield break;
        }
        //else
        //{
        //    // put player to the portal at destinationTag
        //    player = GameManager.Instance.playerStats.gameObject;
        //    //playerAgent = GameObject.FindGameObjectWithTag("Player").transform;
        //    //playerAgent.enable = false;
        //    player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
        //    yield return null;
        //}
        
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        
        var entrances = FindObjectsOfType<TransitionDestination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null;
    }


    public static void ReloadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(sceneIndex);
    }

    public static void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (sceneIndex >= SceneManager.sceneCount)
        {
            // Do something. E.g. Load your main menu scene
            ReloadScene();

            return;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    public static void QuitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

}
