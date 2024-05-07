using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointPath : MonoBehaviour
{
    public Transform GetWayPoint(int wayPointIndex)
    {
        return transform.GetChild(wayPointIndex);
    }

    public int GetNextWayPointIndex(int currentWatPointIndex)
    {
        return (currentWatPointIndex + 1) % transform.childCount;
    }
}
