using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlatforms : MonoBehaviour
{
    [SerializeField]
    private float transitTime;
    public float TimeToWayPoint {get {return transitTime;}}
    // Start is called before the first frame update
    void Awake()
    {
        Sync();
    }

    void Sync()
    {
        foreach (Transform child in transform)
        {
            var script = child.GetComponent<MovingPlatform>();
            script._synced = true;
        }
    }
}