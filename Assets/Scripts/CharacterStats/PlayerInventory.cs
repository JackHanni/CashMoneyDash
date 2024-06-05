using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCoins {get; private set;}
    public int NumberOfGems { get; private set; }

    public UnityEvent<PlayerInventory> OnCoinCollected;
    public UnityEvent<PlayerInventory> OnGemCollected;

    public void CoinCollected() 
    {
        NumberOfCoins++;
        OnCoinCollected.Invoke(this);
    }

    public void GemCollected()
    {
        Debug.Log("count" + GemControl.control.PickupCount);
        NumberOfGems = GemControl.control.PickupCount;
        OnGemCollected.Invoke(this);
    }

}
