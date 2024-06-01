using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public GameObject healthPrefab;

    #region Read from Data_SO
    // readable writable properties
    public int MaxHealth
    {
        get{if(characterData!=null) return characterData.maxHealth; else return 0;}
        set{characterData.maxHealth = value;}
    }

    public int CurrentHealth
    {
        get{if(characterData!=null) return characterData.currentHealth; else return 0;}
        set{characterData.currentHealth = value;}
    }

    public int BaseDefense
    {
        get{if(characterData!=null) return characterData.baseDefense; else return 0;}
        set{characterData.baseDefense = value;}
    }

    public int CurrentDefense
    {
        get{if(characterData!=null) return characterData.currentDefense; else return 0;}
        set{characterData.currentDefense = value;}
    }
    #endregion


    public void PlayerTakeDamage(int damage)
    {
        // update character data
        characterData.currentHealth -= damage;

        // notify UI
        HealthBar healthBar = healthPrefab.GetComponent<HealthBar>();
        healthBar.TakeDamage(damage);
    }

    public void PlayerHeal(int heal)
    {
        if (characterData.currentHealth < characterData.maxHealth) {
            // update character data
            characterData.currentHealth += heal;
            // notify UI
            HealthBar healthBar = healthPrefab.GetComponent<HealthBar>();
            healthBar.Heal(heal);
        }
    }

}
