using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //public static GameManager manager;
    int sceneNumber;
    public CharacterStats playerStats;
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

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
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        sceneNumber = 0;
    }

    void Update() 
    {
        if (sceneNumber < 3 && Input.GetKeyDown(KeyCode.Z)) {
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

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }


    // TODO: Enemies add broadcast notify methods


    // for each observer, execute EndNotify() in enemycontroller
    public void NotifyObservers()
    {
        foreach(var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}