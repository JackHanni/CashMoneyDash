using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{

    public enum SoundEnum
    {
        ENTER, //0 - here we go

        SINGLE_JUMP, // waha
        DOUBLE_JUMP, // woohoo
        DOUBLE_JUMP_1, // whoa
        TRIPLE_JUMP, // yahoo
        TRIPLE_JUMP_1, //5 - yippee

        BACKFLIP, // boing
        LONG_JUMP, // mamamia

        FALL, // 
        DIE, //
        GAME_OVER,
        VICTORY // okie-dokie

    }


    public AudioClip[] sounds;
}


