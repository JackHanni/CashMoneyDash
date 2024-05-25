using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Slime : MonoBehaviour
{
    public int hp = 1;
    public Animator animator;
    public VisualEffect deathEffect;
    public GameObject spawnObject;
    public GameObject path;

    public void TakeDamage(int damageAmount) 
    {
        hp -= damageAmount;
        if (hp <= 0){
            //Play death animation
            animator.SetTrigger("die");
            var colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders) {
                collider.enabled = false;
            }
            animator.SetTrigger("dead");
        } else {
            //Play get hit animation
            animator.SetTrigger("damage");
        }
    }

    public void Kill()
    {
        Instantiate(deathEffect,transform.position, transform.rotation);
        Instantiate(spawnObject,transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
