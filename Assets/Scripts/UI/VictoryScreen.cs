using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelClearedEventChannel;
    [SerializeField] StringEventChannel clearTimeTextEventChannel;
    [SerializeField] AudioClip[] sound;
    [SerializeField] Text timeText;


    private void OnEnable()
    {
        levelClearedEventChannel.AddListener(ShowUI);
        clearTimeTextEventChannel.AddListener(SetTimeText);
    }

    private void OnDisable()
    {
        levelClearedEventChannel.RemoveListener(ShowUI);
        clearTimeTextEventChannel.RemoveListener(SetTimeText);
    }

    void SetTimeText(string obj)
    {
        timeText.text = obj;
    }

    void ShowUI()
    {
        GetComponent<Canvas>().enabled = true;
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;

        AudioClip victorySound = sound[Random.Range(0, sound.Length)];
        SoundEffectsPlayer.AudioSource.PlayOneShot(victorySound);

        Cursor.lockState = CursorLockMode.None;

        float animatorLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float waitTime = Mathf.Max(animatorLength, victorySound.length);

        // wait some time for animation and effects, then exit to main with animation
        StartCoroutine(LoadMainSceneAfterDelay(waitTime));
    }

    IEnumerator LoadMainSceneAfterDelay(float time)
    {
        yield return new WaitForSecondsRealtime(time + 1);
        SceneController.LoadMainScene(); // exit to main with animation
    }
}
