using System.Collections;
using UnityEngine;



public class DefeatScreen : MonoBehaviour
{
    [SerializeField] VoidEventChannel playerDefeatedEventChannel;
    [SerializeField] AudioClip[] sound;

    //[SerializeField] Button retryButton;
    //[SerializeField] Button quitButton;

    void OnEnable()
    {
        playerDefeatedEventChannel.AddListener(ShowUI);

        //retryButton.onClick.AddListener(SceneController.ReloadScene);
        //quitButton.onClick.AddListener(SceneController.QuitGame);
    }

    void OnDisable()
    {
        playerDefeatedEventChannel.RemoveListener(ShowUI);

        //retryButton.onClick.RemoveListener(SceneController.ReloadScene);
        //quitButton.onClick.RemoveListener(SceneController.QuitGame);
    }

    void ShowUI()
    {
        GetComponent<Canvas>().enabled = true;
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;

        AudioClip defeatSound = sound[Random.Range(0, sound.Length)];
        SoundEffectsPlayer.AudioSource.PlayOneShot(defeatSound);

        Cursor.lockState = CursorLockMode.None;


        float animatorLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float waitTime = Mathf.Max(animatorLength, defeatSound.length);

        // wait some time for animation and effects, then exit to main with animation
        StartCoroutine(LoadMainSceneAfterDelay(waitTime));
    }

    IEnumerator LoadMainSceneAfterDelay(float time)
    {
        yield return new WaitForSecondsRealtime(time + 1);
        SceneController.LoadMainScene(); // exit to main with animation
    }
}

