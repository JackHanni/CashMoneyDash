using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //public GameObject pickupEffect;
    public AudioClip audioEffect;


    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            gameObject.SetActive(false);

            // play audio
            AudioSource.PlayClipAtPoint(audioEffect, playerInventory.transform.position);

            // show pick up effects - win
            //Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
