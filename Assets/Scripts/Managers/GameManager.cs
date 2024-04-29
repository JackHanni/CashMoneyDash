using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    int sceneNumber;

    private void Awake()
    {
        if (manager == null) {
            manager = this;
            DontDestroyOnLoad(this);
        } else if(manager != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sceneNumber = 0;
    }

    void Update() 
    {
        if (sceneNumber < 3 && Input.GetKeyDown(KeyCode.Space)) {
            NextScene();
        }
    }

    public void NextScene()
    {
        sceneNumber++;
        if (sceneNumber > 3) {
            sceneNumber = 0;
        }
        SceneManager.LoadScene(sceneNumber);
    }
}