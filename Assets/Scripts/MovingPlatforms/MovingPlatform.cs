using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private WayPointPath _wayPointPath;

    [SerializeField]
    private float _speed;
    private int _targetWayPointIndex;
    private Transform _previousWayPoint;
    private Transform _targetWayPoint;

    private float _timeToWayPoint;
    private float _elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _elapsedTime += .02f;
        float elapsedPercentage = _elapsedTime / _timeToWayPoint;
        elapsedPercentage = Mathf.SmoothStep(0,1,elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWayPoint.position, _targetWayPoint.position, elapsedPercentage);

        if (elapsedPercentage >= 1.0f){
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        _previousWayPoint = _wayPointPath.GetWayPoint(_targetWayPointIndex);
        _targetWayPointIndex = _wayPointPath.GetNextWayPointIndex(_targetWayPointIndex);
        _targetWayPoint = _wayPointPath.GetWayPoint(_targetWayPointIndex);
        _elapsedTime = 0;
        float distanceToWayPoint = Vector3.Distance(_previousWayPoint.position, _targetWayPoint.position);
        _timeToWayPoint = distanceToWayPoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Parented");
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Not");
        other.transform.SetParent(null);
    }
}
