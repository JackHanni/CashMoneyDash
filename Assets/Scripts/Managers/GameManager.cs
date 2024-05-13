using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //public static GameManager manager;
    int sceneNumber;
    public CharacterStats playerStats;

    protected override void Awake()
    {
        //if (manager == null) {
        //    manager = this;
        //    DontDestroyOnLoad(this);
        //} else if(manager != this) {
        //    Destroy(gameObject);
        //}
        base.Awake();
        DontDestroyOnLoad(this);
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

    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }
}