using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject sceneController;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (gameIsPaused){
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        sceneController.GetComponent<SceneController>().LockCursor();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
        sceneController.GetComponent<SceneController>().UnlockCursor();
    }

    public void QuitGame()
    {
        SceneController.QuitGame();
    }

    public void ExitLevel()
    {
        Debug.Log("Exit Level...");
    }


}
