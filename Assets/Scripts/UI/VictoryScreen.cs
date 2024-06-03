using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelClearedEventChannel;

    private void OnEnable()
    {
        levelClearedEventChannel.AddListener(ShowUI);
    }

    private void OnDisable()
    {
        levelClearedEventChannel.RemoveListener(ShowUI);
    }

    void ShowUI()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<Animator>().enabled = true;
    }

    
}
