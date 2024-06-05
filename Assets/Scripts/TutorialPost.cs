using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPost : MonoBehaviour
{
    [SerializeField]
    protected GameObject tutorialCanvas;

    void OnTriggerEnter()
    {
        tutorialCanvas.SetActive(true);
    }

    void OnTriggerExit()
    {
        tutorialCanvas.SetActive(false);
    }

}
