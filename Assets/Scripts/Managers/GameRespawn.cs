using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    public float threshold = -6;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (characterController.transform.position.y < threshold)
        {
            Vector3 resetPosition = new Vector3(0, 1, 0);
            characterController.enabled = false;
            characterController.transform.position = resetPosition;
            characterController.enabled = true;
        }
    }
}



