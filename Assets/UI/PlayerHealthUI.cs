using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    Text levelText;
    Image healthSlider;
    Image expSlider;

    void Awake()
    {
        healthSlider = transform.GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetComponent<Image>();
        levelText = transform.GetChild(2).GetComponent<Text>();
    }

    void Update()
    {
        levelText.text = "Level " + GameManager.Intance.playerStats.characterData.currentLevel.ToString("00"); // e.g. Level 01, Level 02, ... 
        UpdateHealth();
        UpdateExp();
    }


    void UpdateHealth()
    {
        float sliderPercent = (float) GameManager.Intance.playerStats.CurrentHealth / GameManager.Intance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateExp()
    {
        float sliderPercent = ( float ) GameManager.Intance.playerStats.characterData.currentExp / GameManager.Intance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
