using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int hp = 1;
    public Animator animator;

    public void TakeDamage(int damageAmount) 
    {
        hp -= damageAmount;
        if (hp <= 0){
            //Play death animation
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            animator.SetTrigger("dead");
        } else {
            //Play get hit animation
            animator.SetTrigger("damage");
        }
    }
}
