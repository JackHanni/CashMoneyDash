using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    
    [SerializeField] VoidEventChannel levelClearedEventChannel;
    [SerializeField] AudioClip pickUpSFX;
    [SerializeField] GameObject pickUpVFX;

    public event System.Action Delegate;

    void OnTriggerEnter(Collider other)
    {
        levelClearedEventChannel.Broadcast();
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.GemCollected();
            gameObject.SetActive(false);

            // show pick up effects - win
            Instantiate(pickUpVFX, transform.position, transform.rotation);

            AudioSource.PlayClipAtPoint(pickUpSFX, other.transform.position);
            Destroy(gameObject);
        }
        
    }

}
