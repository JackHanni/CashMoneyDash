using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //public GameObject pickupEffect;
    public AudioClip audioEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            // play audio
            AudioSource.PlayClipAtPoint(audioEffect, other.transform.position);

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            gameObject.SetActive(false);
           
            // show pick up effects - win
            //Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
