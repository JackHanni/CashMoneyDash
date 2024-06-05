using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{ 
    [SerializeField] VoidEventChannel levelClearedEventChannel;
    [SerializeField] AudioClip pickUpSFX;
    [SerializeField] GameObject pickUpVFX;
    private GemStates gem;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("tag" + gameObject.tag);
        gem = (GemStates)Enum.Parse(typeof(GemStates), gameObject.tag);

        levelClearedEventChannel.Broadcast();

        // show pick up effects - win
        Instantiate(pickUpVFX, transform.position, transform.rotation);

        // play pick up sound
        AudioSource.PlayClipAtPoint(pickUpSFX, other.transform.position);
        Destroy(gameObject);


        // when colliding with player
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            // increase count in UI
            playerInventory.GemCollected();
            gameObject.SetActive(false);
        }

        // change state and count in gem control
        GemControl.control.gemStates[gem] = true;
        Debug.Log("ok, gem state" + GemControl.control.gemStates[gem]);
        GemControl.control.PickupCount += 1;

        // destroy gem object forever
        SceneController.MarkObjectAsDestroyed(gameObject.name);
        Destroy(gameObject);
    }


    //void Update()
    //{
    //    // for each update, if state set to true
    //    if (GemControl.control.gemStates[gem])
    //    {
    //        Destroy(GemControl.control.gemObjects[gem]);
    //        GemControl.control.gemObjects[gem] = null; // Avoid trying to destroy the object again
    //    }
    //}

}
