using System;
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


    [Header("Kill")]
    public int killPoint;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public int levelBuff;


    public float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LevelUp();
    }

    private void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);  // ensure currentLevel + 1 within 0 to max level 
        baseExp += (int)(baseExp * LevelMultiplier);

        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;


    }
}
