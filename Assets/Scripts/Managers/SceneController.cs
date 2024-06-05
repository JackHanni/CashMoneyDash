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
    private static HashSet<string> destroyedObjects = new HashSet<string>();

    protected override void Awake()
    {
        base.Awake();
        // is this line working?
        DontDestroyOnLoad(this);
        LockCursor();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var gemName in destroyedObjects)
        {
            GameObject gem = GameObject.Find(gemName);
            if (gem != null)
            {
                Destroy(gem);
            }
        }
    }

    public static void MarkObjectAsDestroyed(string gemName)
    {
        destroyedObjects.Add(gemName);
        PlayerPrefs.SetString("DestroyedObjects", string.Join(",", destroyedObjects));
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void TransitionToDestination(TransitionPoint transitionPoint){
        // load next scene based on portal
        SceneManager.LoadScene(transitionPoint.sceneName);
    }

    public static void ReloadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(sceneIndex);
    }

    public static int FindCurrentSceneIdx()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        return sceneIndex;
    }

    public static void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex >= SceneManager.sceneCount)
        {
            ReloadScene();
            return;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public static void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void LoadAllGemClear()
    {
        SceneManager.LoadScene(3);
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
