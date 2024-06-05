using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    [SerializeField] AudioClip startVoice;
    [SerializeField] VoidEventChannel levelStartedEventChannel;

    void LevelStart()
    {
        levelStartedEventChannel.Broadcast();

        GetComponent<Canvas>().enabled = false;
        GetComponent<Animator>().enabled = false;
    }


    void PlayerStartVoice()
    {
        AudioSource.PlayClipAtPoint((AudioClip)startVoice, SoundEffectsPlayer.AudioSource.transform.position);
    }
}
