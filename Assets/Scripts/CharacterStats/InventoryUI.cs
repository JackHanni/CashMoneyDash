using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private TextMeshProUGUI gemText;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerInventory.OnCoinCollected.AddListener(UpdateCoinText);
        playerInventory.OnGemCollected.AddListener(UpdateGemText);
        gemText = GameObject.Find("GemText")?.GetComponent<TextMeshProUGUI>();
        coinText = GameObject.Find("CoinText")?.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCoinText(PlayerInventory playerInventory)
    {
        coinText.text = playerInventory.NumberOfCoins.ToString();
    }

    public void UpdateGemText(PlayerInventory playerInventory)
    {
        gemText.text = playerInventory.NumberOfGems.ToString();
        //Debug.Log("gem text:" + gemText.text);
    }


}
