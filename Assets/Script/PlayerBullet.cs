using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Variables to set damage, speed, and maximum travel distance of the bullet
    public int damage;
    public float Speed;
    public float arrawDistance;

    // Private variables for Rigidbody2D, initial position, player's transform, and direction
    private Rigidbody2D rg2d;
    private Vector3 startPos;
    private Transform playerTrans; // Weapon return position
    private float throwMoment;

    // Initialization
    void Start()
    {
        // Get the player's transform and direction of the throw
        playerTrans = FinalMovement.instance.transform;
        throwMoment = playerTrans.localScale.x;

        // Get the Rigidbody2D component attached to the bullet
        rg2d = GetComponent<Rigidbody2D>();

        // Set the bullet's velocity based on the throw direction
        if (throwMoment == 1)
        {
            rg2d.velocity = transform.right * Speed; 
        }
        else if (throwMoment == -1)
        {
            rg2d.velocity = transform.right * -Speed;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Record the initial position of the bullet
        startPos = transform.position;
    }

    // Called once per frame
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
        // Check if the bullet hit an enemy
        if (other.tag == "Enemy") // FIXME: isDamage [Avoid the axe stopping on the ground but still damaging the enemy]
        {
            // Shake the camera on hit
            AttackScene.Instance.CameraShake(0.1f, 0.1f);

            // Apply hit effects based on the type of enemy and throw direction
            if (other.GetComponent<Enemy>())
            {
                if (throwMoment > 0)
                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                else if (throwMoment < 0)
                    other.GetComponent<Enemy>().GetHit(Vector2.left);
            }
            else if (other.GetComponent<NormalEnemy>())
            {
                if (throwMoment > 0)
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.right);
                else if (throwMoment < 0)
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.left);
            }
            else if (other.GetComponent<Alice_AI>())
            {
                if (throwMoment > 0)
                    other.GetComponent<Alice_AI>().GetHit(Vector2.right, damage);
                else if (throwMoment < 0)
                    other.GetComponent<Alice_AI>().GetHit(Vector2.left, damage);
            }

            // Set the isHit flag for FSM enemies
            if (other.GetComponent<FSM>())
            {
                other.GetComponent<FSM>().isHit = true;
            }

            // Reduce the health of Werewolf enemies
            if (other.GetComponent<Werewolf_AI>())
            {
                other.GetComponent<Werewolf_AI>().health -= damage;
            }

            // Apply damage to the enemy
            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(damage);
            else if (other.GetComponent<NormalEnemy>())
                other.GetComponent<NormalEnemy>().TakeDamage(damage);
            
            // Destroy the bullet after hitting the enemy
            Destroy(gameObject);    
        }
    }
}

