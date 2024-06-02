using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSFX;
    [SerializeField] GameObject pickUpVFX;


    private void OnTriggerEnter(Collider other)
    {

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
