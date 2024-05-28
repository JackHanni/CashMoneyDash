using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public float threshold = -20f;
    private CharacterController characterController;
    public Transform[] respawnPoints; // Array of respawn points

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.transform.position.y < threshold)
        {
            Transform nearestRespawnPoint = GetNearestRespawnPoint();
            if (nearestRespawnPoint != null)
            {
                characterController.enabled = false;
                characterController.transform.position = nearestRespawnPoint.position;
                characterController.enabled = true;
            }
        }
    }

    Transform GetNearestRespawnPoint()
    {
        Transform nearestPoint = null;
        float minDistance = float.MaxValue;

        foreach (Transform respawnPoint in respawnPoints)
        {
            float distance = Vector3.Distance(characterController.transform.position, respawnPoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = respawnPoint;
            }
        }

        return nearestPoint;
    }
}
