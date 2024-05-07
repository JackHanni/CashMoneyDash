using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder;
using UnityEngine;

// create subset menu with params
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;

    public int currentHealth;

    public int baseDefense;

    public int currentDefense;

    // Attack Data are in seperate script

}
