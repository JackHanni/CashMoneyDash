using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingIsland : MonoBehaviour
{
    
    private float timeStep = .02f;
    [SerializeField]
    private float rotationSpeed = 20;
    private Vector3 angularVelocity = Vector3.zero;
    public Vector3 AngularVelocity {get {return angularVelocity;}}
    public Vector3 Center {get {return transform.position;}}
    // Update is called once per frame
    void Start()
    {
        angularVelocity.y = timeStep*rotationSpeed;
    }
    void FixedUpdate()
    {
        transform.Rotate(0,timeStep*rotationSpeed,0,Space.Self);
    }
}
