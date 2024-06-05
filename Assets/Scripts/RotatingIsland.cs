using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingIsland : MonoBehaviour
{
    
    private static float timeStep = .02f;
    private static float rotationSpeed = 20;
    private Vector3 angularVelocity = new Vector3(0.0f,timeStep*rotationSpeed,0.0f);
    public Vector3 AngularVelocity {get {return angularVelocity;}}
    public Vector3 Center {get {return transform.position;}}
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0,timeStep*rotationSpeed,0,Space.Self);
    }
}
