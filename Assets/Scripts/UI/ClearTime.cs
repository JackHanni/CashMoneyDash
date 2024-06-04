using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClearTime : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] VoidEventChannel levelStartedEventChannel;
    [SerializeField] VoidEventChannel levelClearedEventChannel;
    [SerializeField] VoidEventChannel playerDefeatedEventChannel;
    [SerializeField] StringEventChannel clearTimeTextEventChannel;

    float clearTime;

    bool stop = true; // weather increase timer or not


    private void FixedUpdate() // call 50 times per second
    {
        if (stop) return;
        clearTime += Time.fixedDeltaTime; // 0.02
        timeText.text = System.TimeSpan.FromSeconds(value: clearTime).ToString(format: @"mm\:ss\:ff");
    }


    void OnEnable()
    {
        levelStartedEventChannel.AddListener(LevelStart);
        levelClearedEventChannel.AddListener(LevelClear);
        playerDefeatedEventChannel.AddListener(HideUI);
    }

    void OnDisable()
    {
        levelStartedEventChannel.RemoveListener(LevelStart);
        levelClearedEventChannel.RemoveListener(LevelClear);
        playerDefeatedEventChannel.RemoveListener(HideUI);
    }

    void LevelStart()
    {
        stop = false;
    }

    void LevelClear()
    {
        HideUI();
        clearTimeTextEventChannel.Broadcast(timeText.text);
    }

    void HideUI()
    {
        stop = true;
        GetComponent<Canvas>().enabled = false;
    }
}
