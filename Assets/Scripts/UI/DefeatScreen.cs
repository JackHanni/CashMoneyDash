using UnityEngine;
using UnityEngine.UI;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] VoidEventChannel playerDefeatedEventChannel;
    [SerializeField] AudioClip[] voice;

    [SerializeField] Button retryButton;
    [SerializeField] Button quitButton;

    void OnEnable()
    {
        playerDefeatedEventChannel.AddListener(ShowUI);

        retryButton.onClick.AddListener(SceneController.ReloadScene);
        quitButton.onClick.AddListener(SceneController.QuitGame);
    }

    void OnDisable()
    {
        playerDefeatedEventChannel.RemoveListener(ShowUI);

        retryButton.onClick.RemoveListener(SceneController.ReloadScene);
        quitButton.onClick.RemoveListener(SceneController.QuitGame);
    }

    void ShowUI()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<Animator>().enabled = true;

        AudioClip retryVoice = voice[Random.Range(0, voice.Length)];
        SoundEffectsPlayer.AudioSource.PlayOneShot(retryVoice);

        Cursor.lockState = CursorLockMode.None;
    }
}