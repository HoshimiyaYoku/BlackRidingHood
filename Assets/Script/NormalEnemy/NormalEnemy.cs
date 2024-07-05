using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    // Serialized fields for enemy properties
    [SerializeField] protected private string enemyName;
    [SerializeField] protected private float moveSpeed;
    [SerializeField] protected private float backSpeed;
    [SerializeField] protected private float healthPoint;
    [SerializeField] protected private float maxHealthPoint;
    [SerializeField] protected private int damage;
    [SerializeField] protected private float distance;

    // Protected fields for components and state flags
    protected private BoxCollider2D bx2D;
    protected private bool isHit;
    protected private Transform target;
    protected private Vector2 direction;
    new protected private Rigidbody2D rigidbody;
    protected private Animator animator;
    protected private Animator hitAnimator;
    protected private bool isDead;
    protected private bool isAttack;

    // Public fields for attack and detection points
    public Transform attackPoint;
    public float attackArea;
    public Transform enemyCheckPoint;
    public float enemyCheckArea;
    public LayerMask targetLayer;
    public AudioSource hitAudio;

    // Initialization
    private void Start()
    {
        // Initialize health points
        healthPoint = maxHealthPoint;
        
        // Get necessary components
        bx2D = GetComponent<BoxCollider2D>();
        Introduction();
        hitAnimator = transform.Find("HitAnimation").GetComponent<Animator>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
    }

    // Called once per frame
    protected virtual void Update()
    {
        // Get the Animator component and the player target
        animator = transform.GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        // Handle movement and attacks if not hit, attacking, or dead
        if (!isHit && !isAttack && !isDead)
            Move();
        if (!isAttack && !isHit && !isDead)
            Attack();
    }

    // Print enemy introduction message
    private void Introduction()
    {
        Debug.Log("My Name is " + enemyName + ", HP: " + healthPoint + ", moveSpeed: " + moveSpeed);
    }

    // Handle enemy movement
    protected virtual void Move()
    {
        if (Physics2D.OverlapCircle(enemyCheckPoint.position, enemyCheckArea, targetLayer))
        {
            animator.SetBool("isMove", true);
            FlipTo(target);
            this.transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
            animator.SetBool("isMove", false);
    }

    // Handle enemy attacks
    protected virtual void Attack()
    {
        if (Physics2D.OverlapCircle(attackPoint.position, attackArea, targetLayer))
        {
            FlipTo(target);
            animator.SetBool("isMove", false);
            isAttack = true;
            animator.SetBool("isAttack", true);
        }
    }

    // Flip the enemy to face the target
    public void FlipTo(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    // Handle getting hit
    public virtual void GetHit(Vector2 direction)
    {
        hitAudio.Play();
        isHit = true;
        isAttack = false;
        rigidbody.velocity = direction * backSpeed;
        transform.localScale = new Vector3(-direction.x, 1, 1);
        bx2D.enabled = false;
        this.direction = direction;
        animator.SetBool("isMove", false);
        animator.SetTrigger("Hit");
        hitAnimator.SetTrigger("Hit");
        animator.SetBool("isAttack", false);

        // Check if health is depleted
        if (healthPoint <= 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            isDead = true;
            animator.SetTrigger("Die");
            this.transform.tag = "Untagged";
        }
    }

    // Apply damage to the enemy
    public void TakeDamage(int damage)
    {
        healthPoint -= damage;
    }

    // Handle collision with the player
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isDead)
        {
            if (FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
        }
    }

    // Reset state after hit
    void StartMove()
    {
        isHit = false;
        bx2D.enabled = true;
        animator.SetBool("isMove", true);
    }

    // Handle enemy death
    protected virtual void DeathTime()
    {
        Destroy(gameObject);
    }

    // Reset state after attack
    void AttackOver()
    {
        isAttack = false;
        animator.SetBool("isAttack", false);
        animator.SetBool("isMove", true);
    }

    // Draw gizmos for attack and enemy detection areas
    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackArea);
        if (enemyCheckPoint != null)
            Gizmos.DrawWireSphere(enemyCheckPoint.position, enemyCheckArea);
    }
}



