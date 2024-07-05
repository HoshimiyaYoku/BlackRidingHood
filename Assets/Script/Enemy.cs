using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Public variables for enemy properties
    public float speed;
    public float invisibleTime;
    public int health;
    public int damage;
    private Vector2 direction;
    private bool isHit;
    private AnimatorStateInfo info;
    private float timer = 10f;
    private Animator animator;
    private Animator hitAnimator;
    private new Rigidbody2D rigidbody;
    private BoxCollider2D collider2D;
    private FinalMovement playerHealth;
    private bool isDead;
    public AudioSource hitAudio;

    public void Start()
    {
        // Initialize references to player health, animator, rigidbody, and collider
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalMovement>();
        animator = transform.GetComponent<Animator>();
        hitAnimator = transform.Find("HitAnimation").GetComponent<Animator>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        // Check if enemy health is depleted and handle death
        if (health <= 0 && timer >= 0)
        {
            collider2D.enabled = false;
            isDead = true;
            // animator.SetTrigger("Dead");
            timer -= Time.deltaTime;
        }
        else if (health <= 0 && timer <= 0)
        {
            Destroy(gameObject);
        }

        // Update animation state and check if enemy is hit
        info = animator.GetCurrentAnimatorStateInfo(0);
        if (isHit)
        {
            rigidbody.velocity = direction * speed;
            if (info.normalizedTime >= .95f)
                isHit = false;
        }
    }

    // Handle the enemy getting hit
    public void GetHit(Vector2 direction)
    {
        hitAudio.Play();
        transform.localScale = new Vector3(-direction.x, 1, 1);
        isHit = true;
        collider2D.enabled = false;
        Invoke("Invisible", invisibleTime);
        this.direction = direction;
        animator.SetTrigger("Hit");
        hitAnimator.SetTrigger("Hit");
    }

    // Apply damage to the enemy
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    // Handle collision with the player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
        }
    }

    // Handle invincibility period after getting hit
    void Invisible()
    {
        collider2D.enabled = true;
    }
}
