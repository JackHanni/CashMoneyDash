using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject pickupEffect;
    public AudioClip audioEffect;


    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            // increment coin count
            playerInventory.CoinCollected();
            gameObject.SetActive(false);

            // play audio
            AudioSource.PlayClipAtPoint(audioEffect, playerInventory.transform.position);

            // show pick up effects
            Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }


    }
}
