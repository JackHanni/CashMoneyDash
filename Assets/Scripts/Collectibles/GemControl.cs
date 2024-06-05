using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum GemStates
{
    Untagged,
    gem1,
    gem2,
    gem3,
    gem4,
    gem5,
    gem6,
    gem7,
    gem8,
    gem9,
    gem10,
}

public class GemControl : MonoBehaviour
{
    public static GemControl control;
    public int PickupCount;
    //[SerializeField] TextMeshProUGUI gemCount;

    // level 1
    public bool Gem1State = false;
    public bool Gem2State = false;
    public bool Gem3State = false;
    public bool Gem4State = false;
    public bool Gem5State = false;

    // level 2
    public bool Gem6State = false;
    public bool Gem7State = false;
    public bool Gem8State = false;
    public bool Gem9State = false;
    public bool Gem10State = false;


    public Dictionary<GemStates, bool> gemStates;
    //private PlayerInventory playerInventory;


    //void Start()
    //{
    //    playerInventory = FindObjectOfType<PlayerInventory>();
    //    playerInventory.OnGemCollected.AddListener(UpdateUI);
    //}

    //private void UpdateUI(PlayerInventory playerInventory)
    //{
    //    gemCount.text = PickupCount.ToString();
    //}

    void Awake()
    {
        if (control == null)
        { 
            DontDestroyOnLoad(gameObject);
            control = this;

            gemStates = new Dictionary<GemStates, bool> {
                { GemStates.gem1, Gem1State },
                { GemStates.gem2, Gem2State },
                { GemStates.gem3, Gem3State },
                { GemStates.gem4, Gem4State },
                { GemStates.gem5, Gem5State },
                { GemStates.gem6, Gem6State },
                { GemStates.gem7, Gem7State },
                { GemStates.gem8, Gem8State },
                { GemStates.gem9, Gem9State },
                { GemStates.gem10,Gem10State }
            };

        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (PickupCount == 10)
        {
            Debug.Log("Good!");
        }
    }



    void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        style.fontSize = Mathf.RoundToInt(Screen.height * 0.03f);
        style.normal.textColor = Color.white;

        float screenPadding = 20f;

        float labelWidth = 200;
        float labelHeight = 50;
        float labelX = Screen.width - labelWidth - screenPadding;
        float labelY = 10f;

        GUI.Label(new Rect(labelX, labelY, labelWidth, labelHeight), "Gem Counts: " + PickupCount + "/10");

    }


    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        GemData data = new GemData();
        data.PickupCount = PickupCount;
        data.GemStates = new List<bool>(gemStates.Values);

        bf.Serialize(file, data);
        file.Close();
    }


    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            GemData data = (GemData)bf.Deserialize(file);
            file.Close();

            PickupCount = data.PickupCount;
            int i = 0;
            foreach (GemStates gemState in gemStates.Keys)
            {
                gemStates[gemState] = data.GemStates[i];
                i++;
            }
        }
    }
}


[Serializable]
class GemData
{
    public int PickupCount;
    public List<bool> GemStates;
}
