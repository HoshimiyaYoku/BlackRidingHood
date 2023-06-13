using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
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
    new private Rigidbody2D rigidbody;
    private BoxCollider2D collider2D;
    private FinalMovement playerHealth;
    private bool isDead;
    public AudioSource hitAudio;

    public void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalMovement>();
        animator = transform.GetComponent<Animator>();
        hitAnimator = transform.Find("HitAnimation").GetComponent<Animator>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        if (health <= 0 && timer >= 0)
        {
            collider2D.enabled = false;
            isDead = true;
            //animator.SetTrigger("Dead");

            timer -= Time.deltaTime;
        }
        else if(health <= 0 && timer <= 0)
        {
            Destroy(gameObject);
            
        }

        info = animator.GetCurrentAnimatorStateInfo(0);     // 判断受伤
        if (isHit)
        {
            rigidbody.velocity = direction * speed;
            if (info.normalizedTime >= .95f)
                isHit = false;
        }
    }

    public void GetHit(Vector2 direction)       // 受击
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

    public void TakeDamage(int damage)      //受到伤害量
    {
        health -= damage;
    }

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

     void Invisible()        // 无敌时间
    {
        collider2D.enabled = true;
    }
}
