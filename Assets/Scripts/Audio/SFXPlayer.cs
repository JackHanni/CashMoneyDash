using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{

    public enum SoundEnum
    {
        GREETING, //0 - let's go
        GREETING_1, // yoohoo


        SINGLE_JUMP, // whistle
        DOUBLE_JUMP, // woo
        TRIPLE_JUMP, // wee3

        BACKFLIP, // wow - meme 
        LONG_JUMP, // whooo - meme

        FALL, // uh-oh
        DIE, // disappoint
        VICTORY // yay

    }


    public AudioClip[] sounds;
}


