using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHits : MonoBehaviour
{
    // Variables for damage and maximum travel distance of the bullet
    public int damage;
    public float arrawDistance;

    // Reference to the player's health component
    private FinalMovement playerHealth;

    // Rigidbody2D component and initial position of the bullet
    private Rigidbody2D rg2d;
    private Vector3 startPos;

    // Use this for initialization
    void Start()
    {
        // Get the FinalMovement component of the player
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalMovement>();

        // Get the Rigidbody2D component attached to the bullet
        rg2d = GetComponent<Rigidbody2D>();

        // Record the initial position of the bullet
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance traveled by the bullet
        float distance = (transform.position - startPos).sqrMagnitude;

        // Destroy the bullet if it has traveled beyond the maximum distance
        if (distance > arrawDistance)
        {
            Destroy(gameObject);
        }
    }

    // Called when the bullet collides with another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet hit the player
        if(other.gameObject.tag == "Player") 
        {
            if(FinalMovement.instance != null)
            {
                // Apply damage to the player
                FinalMovement.instance.DamagePlayer(damage);
            }
        } 
        
        // Destroy the bullet if it collides with a player bullet
        if(other.gameObject.tag == "PlayerBullet") 
        {
            Destroy(gameObject);
        }      
    }
}

