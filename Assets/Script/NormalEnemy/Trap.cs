using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{   
    [SerializeField] private int damage;
    void OnTriggerEnter2D(Collider2D other)     //攻击判定
    {
        if(other.gameObject.tag == "Player") 
        {
            if(FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
        }   
    }
}
