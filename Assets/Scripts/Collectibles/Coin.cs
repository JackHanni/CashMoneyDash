using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject pickupEffect;
    public AudioClip audioEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            // play audio
            AudioSource.PlayClipAtPoint(audioEffect, other.transform.position);

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            // increment coin count
            playerInventory.CoinCollected();
            gameObject.SetActive(false);

            
            // show pick up effects
            Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }


    }
}
