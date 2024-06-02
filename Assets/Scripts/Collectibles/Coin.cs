using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSFX;
    [SerializeField] GameObject pickUpVFX;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            // increment coin count
            playerInventory.CoinCollected();
            gameObject.SetActive(false);

            // show pick up effects
            Instantiate(pickUpVFX, transform.position, transform.rotation);


            AudioSource.PlayClipAtPoint(pickUpSFX, other.transform.position);
            Destroy(gameObject);
        }
        // heal player for 1
        CharacterStats stats = other.GetComponent<CharacterStats>();
        stats.PlayerHeal(1);

    }
}
