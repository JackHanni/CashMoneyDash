using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject healthPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SmashableEnemies")
        {
            // execute the take damage method in healthPrefab
            HealthBar healthBar = healthPrefab.GetComponent<HealthBar>();
            healthBar.TakeDamage();
        }
    }
}
