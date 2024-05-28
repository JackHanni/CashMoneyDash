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
        NumberOfGems++;
        OnGemCollected.Invoke(this);
    }

}
