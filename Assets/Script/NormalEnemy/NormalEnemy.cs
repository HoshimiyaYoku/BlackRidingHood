using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    [SerializeField] protected private string enemyName;
    [SerializeField] protected private float moveSpeed;
    [SerializeField] protected private float backSpeed;
    [SerializeField] protected private float healthPoint;
    [SerializeField] protected private float maxHealthPoint;
    [SerializeField] protected private int damage;
    [SerializeField] protected private float distance;
    protected private BoxCollider2D bx2D;
    protected private bool isHit;
    protected private Transform target;
    protected private Vector2 direction;
    new protected private Rigidbody2D rigidbody;
    protected private Animator animator;
    protected private Animator hitAnimator;
    protected private bool isDead;
    protected private bool isAttack;
    public Transform attackPoint;
    public float attackArea;
    public Transform enemyCheckPoint;
    public float enemyCheckArea;
    public LayerMask targetLayer;
    public AudioSource hitAudio;



    private void  Start() 
    {
        healthPoint = maxHealthPoint;
        bx2D = GetComponent<BoxCollider2D>();
        Introduction();
        hitAnimator = transform.Find("HitAnimation").GetComponent<Animator>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        animator = transform.GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if(!isHit && !isAttack && !isDead)
            Move();  
        if(!isAttack && !isHit && !isDead)
            Attack();
    }
   
    private void Introduction()
    {
        Debug.Log("My Name is " + enemyName + ", HP: "+ healthPoint + ", moveSpeed: " + moveSpeed);
    }

    protected virtual void Move()
    {
        if(Physics2D.OverlapCircle(enemyCheckPoint.position, enemyCheckArea, targetLayer))
        {
            animator.SetBool("isMove", true);
            FlipTo(target);
            this.transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
            animator.SetBool("isMove", false);
    }
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

    public virtual void GetHit(Vector2 direction)       // 受击
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
        
        if (healthPoint <= 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            isDead = true;
            animator.SetTrigger("Die");
            this.transform.tag = "Untagged";
        }

    }

    public void TakeDamage(int damage)      //受到伤害量
    {
        healthPoint -= damage;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)     //攻击判定
    {
       if(other.gameObject.tag == "Player" && !isDead) 
        {
            if(FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
        }   
    }


    void StartMove()
    {
        isHit = false;
        bx2D.enabled = true;
        animator.SetBool("isMove", true);  
    }

    protected virtual void DeathTime()    //死亡
    {
        Destroy(gameObject);
    }

    void AttackOver()
    {
        isAttack = false;
        animator.SetBool("isAttack", false);
        animator.SetBool("isMove", true);
    }
    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackArea);
        if (enemyCheckPoint != null)
            Gizmos.DrawWireSphere(enemyCheckPoint.position, enemyCheckArea);
    }

}


