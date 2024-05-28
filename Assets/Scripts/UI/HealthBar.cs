using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// interact with UI for health bar
public class HealthBar : MonoBehaviour
{
    #region Variables
    private float segmentCount;
    private float removeSegments;
    private float segmentSpacing;
    private Color colorA;
    private Color colorB;
    #endregion

    private Renderer renderer;
    private MaterialPropertyBlock propertyBlock;

    //public GameObject player;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    public void TakeDamage()
    {
        Debug.Log("here");
        // dynamically edit Shader Graph material attribute
        removeSegments = propertyBlock.GetFloat("_RemoveSegments");
        propertyBlock.SetFloat("_RemoveSegments", removeSegments + 1);
        renderer.SetPropertyBlock(propertyBlock);
        Debug.Log("here" + removeSegments);
    }
}

