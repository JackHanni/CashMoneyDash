using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_VFX_Player : MonoBehaviour
{

    public enum VoiceEnum
    {
        SINGLE_JUMP, // whistle
        DOUBLE_JUMP, // woo
        TRIPLE_JUMP, // wee3

        BACKFLIP, // wow - meme 
        LONG_JUMP, // whooo - meme
        FALL // uh-oh
    }

    public enum VFXEnum
    {
        JUMP,
        DEFEAT,
        VICTORY
    }

    public AudioClip[] voices;

    public GameObject[] effects;
    public AudioClip[] win_sounds;
    public AudioClip[] lose_sounds;
}

