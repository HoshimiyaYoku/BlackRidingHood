using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHits : MonoBehaviour
{
     public int damage;
    public float arrawDistance;
    private FinalMovement playerHealth;
    private Rigidbody2D rg2d;
    private Vector3 startPos;

    // Use this for initialization
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalMovement>();
        rg2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update()
    {
        float distance = (transform.position - startPos).sqrMagnitude;
        if (distance > arrawDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.tag == "Player") 
        {
            if(FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
        } 
        
        if(other.gameObject.tag == "PlayerBullet") 
        {
            
            Destroy(gameObject);
        
        }      
    }
}
