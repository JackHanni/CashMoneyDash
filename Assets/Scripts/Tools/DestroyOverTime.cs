using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroye game objects after certain amount of time automatically
public class DestroyOverTime : MonoBehaviour
{
    public float lifetime;

    private void Update()
    {
        Destroy(gameObject, lifetime);
    }
    
}
