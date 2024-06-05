using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlatforms : MonoBehaviour
{
    private float timer = 0.0f;
    [SerializeField]
    private float transitTime;
    public float TimeToWayPoint {get {return transitTime;}}
    // Start is called before the first frame update
    void Awake()
    {
        Sync();
        // timer = 0.0f;
        // SetSpeeds();
    }

    void Sync()
    {
        foreach (Transform child in transform)
        {
            var script = child.GetComponent<MovingPlatform>();
            script._synced = true;
        }
    }

    void SetSpeeds()
    {
        foreach (Transform child in transform)
        {
            var script = child.GetComponent<MovingPlatform>();
            var dist = script.Distance;
            Debug.Log("Sync Plat" +dist);
            script.Speed = dist/transitTime;
        }
    }

    void Update()
    {
        // timer += Time.deltaTime;
        // if (timer >= transitTime) {
        //     SetSpeeds();
        //     timer = 0.0f;
        // }
    }
}
