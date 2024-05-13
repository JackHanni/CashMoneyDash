using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System;

// Singleton does not need following two lines
// [System.Serializable] // serializable
// public class EventVector3:UnityEvent<Vector3> {}

public class MouseManager : Singleton<MouseManager>
{
    RaycastHit hitInfo;
    // public EventVector3 OnMouseClicked;

    public event Action<Vector3> OnMouseClicked;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }

    void Update(){
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitInfo)){
            // switch mouse 
        }
    }
    
    void MouseControl(){
        if(Input.GetMouseButtonDown(0) && hitInfo.collider != null){
            // if click on ground, then execute all methods that is in the OnMouseClicked
            if(hitInfo.collider.gameObject.CompareTag("Ground"))
                // all methods registered to this event will be executed
                OnMouseClicked?.Invoke(hitInfo.point);  // syntax: if OnMouseClicked is not null, then invoke
        }
    }
}
