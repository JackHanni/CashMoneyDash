using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePoofBehavior : MonoBehaviour
{
    private float lifetime;
    private float lifespan = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        lifetime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > lifespan) {
            Destroy(gameObject);
        }
    }
}
