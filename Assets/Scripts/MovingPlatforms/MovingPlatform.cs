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
    [SerializeField]
    private Vector3 _currentMovement;

    public Vector3 CurrentMovement {get {return _currentMovement;}}
    private float distance;
    public float Distance {get {return distance;}}
    public float Speed {get {return _speed;} set {_speed = value;}}
    public float TimeToWayPoint {get {return _timeToWayPoint;} set {_timeToWayPoint = value;}}

    public bool _synced = false;
    public GameObject _syncPath;

    // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentPos = transform.position;
        _elapsedTime += .02f;
        float elapsedPercentage = _elapsedTime / _timeToWayPoint;
        elapsedPercentage = Mathf.SmoothStep(0,1,elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWayPoint.position, _targetWayPoint.position, elapsedPercentage);
        _currentMovement = transform.position - currentPos;

        if (elapsedPercentage >= 1.0f){
            TargetNextWaypoint();
        }
    }

    public void TargetNextWaypoint()
    {
        _previousWayPoint = _wayPointPath.GetWayPoint(_targetWayPointIndex);
        _targetWayPointIndex = _wayPointPath.GetNextWayPointIndex(_targetWayPointIndex);
        _targetWayPoint = _wayPointPath.GetWayPoint(_targetWayPointIndex);
        _elapsedTime = 0;
        float distanceToWayPoint = Vector3.Distance(_previousWayPoint.position, _targetWayPoint.position);
        distance = Vector3.Distance(transform.position,_targetWayPoint.position);
        if (!_synced) {
            _timeToWayPoint = distanceToWayPoint / _speed;
        } else if (_syncPath != null){
            var script = _syncPath.GetComponent<SyncPlatforms>();
            _timeToWayPoint = script.TimeToWayPoint;
            _speed = distance/_timeToWayPoint;
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     other.transform.SetParent(transform);
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     other.transform.SetParent(null);
    // }
}
