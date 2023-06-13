using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    [SerializeField] private float shotSpeed;
    [SerializeField] private float maxLife = 2.0f;
    private float lifeBtwTimer;
    public int damage;

    private void Awake() 
    {
        target = GameObject.Find("CheckPoint").transform;    
    }
    
    private void Update() 
    {
        FlipTo(target);
        transform.position = Vector2.MoveTowards(transform.position, target.position, shotSpeed * Time.deltaTime);
        lifeBtwTimer += Time.deltaTime;
        if (lifeBtwTimer >= maxLife)
        {
            Destroy(gameObject);
        }    
    }
    
    
    
    public void FlipTo(Transform target)
    {
        if(target != null)
        {
            if(transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.tag == "Player") 
        {
            if(FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
                Destroy(gameObject);
            }
        } 
        
        if(other.gameObject.tag == "PlayerBullet") 
        {
            
            Destroy(gameObject);
        
        }      
    }
}
