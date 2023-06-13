using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alice_AI : MonoBehaviour
{
    public GameObject atkCandy1;
    public GameObject atkCandy2;
    public GameObject atkCandy3;
    public GameObject atkCandy4;
    public GameObject healCandy1;
    public GameObject healCandy2;
    public GameObject healCandy3;
    public GameObject healCandy4;

    public GameObject LinearLockOn;
    public GameObject SpiralMultiRight;
    public GameObject SpiralMultiLeft;
    public GameObject SpiralRightTurn;
    public GameObject SpiralMix;
    public GameObject _20WayAccelAllRange;
    public GameObject RandomAlice;
    public GameObject SinWaveBullet4WayAiming;
    public GameObject SinWaveBulletAccelTurn;
    public GameObject scissors;

    public GameObject LinearLockOn1;
    public GameObject LinearLockOn2;
    public GameObject LinearLockOn3;
    public GameObject LinearLockOn4;
    public GameObject LinearLockOn5;
    public GameObject LinearLockOn6;
    public GameObject LinearLockOn7;

    public float speed;
    public float invisibleTime;
    public int health = 300;
    public int damage = 8;

    public LayerMask targetMask;
    public Transform attackPoint;
    public float attackArea;
    private Vector2 direction;
    private bool isHit;
    private AnimatorStateInfo info;
    private float timer = 10f;
    public Animator animator;
    private Animator hitAnimator;
    private Rigidbody2D rigidbody;

    public BoxCollider2D collider2D;
    private FinalMovement playerHealth;
    private bool isDead;
    public AudioSource hitAudio;
    private float idletimer = 0f;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Color O_color;
    private float distance;
    private bool isRunning;
    private bool isAttacking;
    private bool isMagic;
    private Vector2 flyPoint;
    private bool candy = false;
    private bool sc = false;
    private float[] stage1= new float[4] {15f,10f,10f,10f};
    //private float[] stage1 = new float[4] { 0f, 5f, 5f, 10f };
    private float[] stage2 = new float[4] { 10f, 5f, 15f, 10f }; 
    private float[] stage3 = new float[5] { 10f, 10f, 10f, 10f ,10f};


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalMovement>();
        target = GameObject.Find("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        flyPoint.x = transform.position.x;
        flyPoint.y = transform.position.y + 20f;
        hitAnimator = transform.Find("HitAnimation").GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        O_color = spriteRenderer.color;
        FlipTo();
    }


    // Update is called once per frame
    void Update()
    {
        if (QuestManager.instance.AliceIsDie)
            gameObject.SetActive(false);
        if(transform.position.x >= target.position.x) distance = transform.position.x - target.position.x;
        if(transform.position.x < target.position.x) distance = target.position.x - transform.position.x;
        if(idletimer >= 0)
        {
            idletimer -= Time.deltaTime;
        }
        else if(health > 0)
        {
            FlipTo();

            if(health > 200 && health <= 300)
            {

                if(stage1[0] > 0)
                {
                    stage1[0] -= Time.deltaTime;
                    if (distance > 2f)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
                        animator.SetBool("running", true);
                        isRunning = true;
                    }
                    else
                    {
                        animator.SetBool("running", false);
                        isRunning = false;
                        if (Physics2D.OverlapCircle(attackPoint.position, attackArea, targetMask) && !isAttacking && !isMagic)
                        {
                            isAttacking = true;
                            animator.SetBool("isAttacking", true);
                            animator.SetTrigger("Attack");
                        }
                    }
                }
                if(stage1[0] <= 0 && stage1[1] > 0 && !isAttacking && !isMagic)
                {
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    LinearLockOn.SetActive(true);
                }
                else if(stage1[0] <= 0 && stage1[1] > 0 && isMagic && stage1[2] > 0)
                {
                    stage1[1] -= Time.deltaTime;
                }
                else if(stage1[0] <= 0 && stage1[1] <= 0 && isMagic && stage1[2] > 0 && LinearLockOn.activeInHierarchy)
                {
                    Debug.Log("4");
                    LinearLockOn.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                }

                if(stage1[0] <= 0 && stage1[1] <= 0 && stage1[2] > 0 && !isAttacking && !isMagic)
                {
                    Debug.Log("1");
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    SpiralRightTurn.SetActive(true);
                }
                else if (stage1[0] <= 0 && stage1[1] <= 0 && stage1[2] > 0 && isMagic)
                {
                    Debug.Log("2");
                    stage1[2] -= Time.deltaTime;
                }
                else if (stage1[0] <= 0 && stage1[1] <= 0 && stage1[2] <= 0 && isMagic && SpiralRightTurn.activeInHierarchy)
                {
                    Debug.Log("3");
                    SpiralRightTurn.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                }

                if (stage1[0] <= 0 && stage1[1] <= 0 && stage1[2] <= 0 && stage1[3] > 0 && !isAttacking && !isMagic)
                {
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    candy = false;
                }
                else if (stage1[0] <= 0 && stage1[1] <= 0 && stage1[2] <= 0 && stage1[3] > 0 && isMagic)
                {
                    stage1[3] -= Time.deltaTime;
                    int j = (int)(stage1[3]);
                    if (j < 8 && j > 6) candy = false;
                    if (j < 6 && j > 4) candy = false;
                    if (j < 4 && j > 2) candy = false;
                    if (j < 2 && j > 0) candy = false;
                    if ((j == 8||j == 6 ||j == 4||j == 2 || j == 0)&& !candy)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            int k = Random.Range(1, 5);
                            if (k == 4)
                            {
                                HealCandy();
                            }
                            else
                            {
                                CandyAtk();
                            }
                        }
                        candy = true;
                    }
                    
                }
                else if (stage1[0] <= 0 && stage1[1] <= 0 && stage1[2] <= 0 && isMagic && stage1[3] <= 0)
                {
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                    stage1[0] = 15f;
                    stage1[1] = 10f;
                    stage1[2] = 10f;
                    stage1[3] = 10f;
                }
            }
            else if(health > 100 && health <= 200)
            {
                if ((LinearLockOn.activeInHierarchy || SpiralRightTurn.activeInHierarchy) && isMagic)
                {
                    LinearLockOn.SetActive(false);
                    SpiralRightTurn.SetActive(false);
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                    isMagic = false;
                }
                if (stage2[0] > 0)
                {
                    stage2[0] -= Time.deltaTime;
                    if (distance > 2f)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
                        animator.SetBool("running", true);
                        isRunning = true;
                    }
                    else
                    {
                        animator.SetBool("running", false);
                        isRunning = false;
                        if (Physics2D.OverlapCircle(attackPoint.position, attackArea, targetMask) && !isAttacking && !isMagic)
                        {
                            isAttacking = true;
                            animator.SetBool("isAttacking", true);
                            animator.SetTrigger("Attack");
                        }
                    }
                }
                if (stage2[0] <= 0 && stage2[1] > 0 && !isAttacking && !isMagic)
                {
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    SinWaveBullet4WayAiming.SetActive(true);
                }
                else if (stage2[0] <= 0 && stage2[1] > 0 && isMagic && stage2[2] > 0)
                {
                    stage2[1] -= Time.deltaTime;
                }
                else if (stage2[0] <= 0 && stage2[1] <= 0 && isMagic && stage2[2] > 0 && SinWaveBullet4WayAiming.activeInHierarchy)
                {
                    Debug.Log("4");
                    SinWaveBullet4WayAiming.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                }

                if (stage2[0] <= 0 && stage2[1] <= 0 && stage2[2] > 0 && !isAttacking && !isMagic)
                {
                    Debug.Log("1");
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    SinWaveBulletAccelTurn.SetActive(true);
                }
                else if (stage2[0] <= 0 && stage2[1] <= 0 && stage2[2] > 0 && isMagic)
                {
                    Debug.Log("2");
                    stage2[2] -= Time.deltaTime;
                }
                else if (stage2[0] <= 0 && stage2[1] <= 0 && stage2[2] <= 0 && isMagic && SinWaveBulletAccelTurn.activeInHierarchy)
                {
                    Debug.Log("3");
                    SinWaveBulletAccelTurn.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                }

                if (stage2[0] <= 0 && stage2[1] <= 0 && stage2[2] <= 0 && stage2[3] > 0 && !isAttacking && !isMagic)
                {
                    RandomAlice.SetActive(true);
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    candy = false;
                }
                else if (stage2[0] <= 0 && stage2[1] <= 0 && stage2[2] <= 0 && stage2[3] > 0 && isMagic)
                {
                    stage2[3] -= Time.deltaTime;
                    int j = (int)(stage2[3]);
                    if (j < 10 && j > 5) candy = false;
                    if (j < 5 && j > 0) candy = false;
                    if ((j == 10 || j == 5 || j == 0) && !candy)
                    {
                        for (int i = 1; i <= 15; i++)
                        {
                            int k = Random.Range(1, 5);
                            if (k == 4)
                            {
                                HealCandy();
                            }
                            else
                            {
                                CandyAtk();
                            }
                        }
                        candy = true;
                    }

                }
                else if (stage2[0] <= 0 && stage2[1] <= 0 && stage2[2] <= 0 && isMagic && stage2[3] <= 0 && RandomAlice.activeInHierarchy)
                {
                    RandomAlice.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                    stage2[0] = 10f;
                    stage2[1] = 5f;
                    stage2[2] = 15f;
                    stage2[3] = 10f;
                }

            }
            else if(health > 0 && health <= 100)//////////
            {
                if ((SinWaveBullet4WayAiming.activeInHierarchy || SinWaveBulletAccelTurn.activeInHierarchy|| RandomAlice.activeInHierarchy) && isMagic)
                {
                    SinWaveBullet4WayAiming.SetActive(false);
                    SinWaveBulletAccelTurn.SetActive(false);
                    RandomAlice.SetActive(false);
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                    isMagic = false;
                }
                if (stage3[0] > 0)
                {
                    stage3[0] -= Time.deltaTime;
                    if (distance > 2f)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
                        animator.SetBool("running", true);
                        isRunning = true;
                    }
                    else
                    {
                        animator.SetBool("running", false);
                        isRunning = false;
                        if (Physics2D.OverlapCircle(attackPoint.position, attackArea, targetMask) && !isAttacking && !isMagic)
                        {
                            isAttacking = true;
                            animator.SetBool("isAttacking", true);
                            animator.SetTrigger("Attack");
                        }
                    }
                }
                if (stage3[0] <= 0 && stage3[1] > 0 && !isAttacking && !isMagic)
                {
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    SpiralMultiLeft.SetActive(true);
                }
                else if (stage3[0] <= 0 && stage3[1] > 0 && isMagic && stage3[2] > 0)
                {
                    stage3[1] -= Time.deltaTime;
                }
                else if (stage3[0] <= 0 && stage3[1] <= 0 && isMagic && stage3[2] > 0 && SpiralMultiLeft.activeInHierarchy)
                {
                    Debug.Log("4");
                    SpiralMultiLeft.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                }

                if (stage3[0] <= 0 && stage3[1] <= 0 && stage3[2] > 0 && !isAttacking && !isMagic)
                {
                    Debug.Log("1");
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    _20WayAccelAllRange.SetActive(true);
                }
                else if (stage3[0] <= 0 && stage3[1] <= 0 && stage3[2] > 0 && isMagic)
                {
                    Debug.Log("2");
                    stage3[2] -= Time.deltaTime;
                }
                else if (stage3[0] <= 0 && stage3[1] <= 0 && stage3[2] <= 0 && isMagic && _20WayAccelAllRange.activeInHierarchy)
                {
                    Debug.Log("3");
                    _20WayAccelAllRange.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                }

                if (stage3[0] <= 0 && stage3[1] <= 0 && stage3[2] <= 0 && stage3[3] > 0 && !isAttacking && !isMagic)
                {
                    LinearLockOn1.SetActive(true);
                    LinearLockOn2.SetActive(true);
                    LinearLockOn3.SetActive(true);
                    LinearLockOn4.SetActive(true);
                    LinearLockOn5.SetActive(true);
                    LinearLockOn6.SetActive(true);
                    LinearLockOn7.SetActive(true);
                    isMagic = true;
                    animator.SetTrigger("Magic");
                    animator.SetBool("isMagic", true);
                    candy = false;
                }
                else if (stage3[0] <= 0 && stage3[1] <= 0 && stage3[2] <= 0 && stage3[3] > 0 && isMagic)
                {
                    stage3[3] -= Time.deltaTime;
                    int j = (int)(stage3[3]);
                    if (j < 10 && j > 5) candy = false;
                    if (j < 5 && j > 0) candy = false;
                    if ((j == 10 || j == 5 || j == 0) && !candy)
                    {
                        for (int i = 1; i <= 15; i++)
                        {
                            int k = Random.Range(1, 5);
                            if (k == 4)
                            {
                                CandyAtk();
                            }
                            else
                            {
                                HealCandy();
                            }
                        }
                        candy = true;
                    }

                }
                else if (stage3[0] <= 0 && stage3[1] <= 0 && stage3[2] <= 0 && isMagic && stage3[3] <= 0 && LinearLockOn1.activeInHierarchy && LinearLockOn2.activeInHierarchy && LinearLockOn3.activeInHierarchy && LinearLockOn4.activeInHierarchy && LinearLockOn5.activeInHierarchy && LinearLockOn6.activeInHierarchy && LinearLockOn7.activeInHierarchy)
                {
                    LinearLockOn1.SetActive(false);
                    LinearLockOn2.SetActive(false);
                    LinearLockOn3.SetActive(false);
                    LinearLockOn4.SetActive(false);
                    LinearLockOn5.SetActive(false);
                    LinearLockOn6.SetActive(false);
                    LinearLockOn7.SetActive(false);
                    isMagic = false;
                    animator.SetBool("isMagic", false);
                    animator.SetTrigger("MagicOver");
                    stage3[0] = 10f;
                    stage3[1] = 10f;
                    stage3[2] = 15f;
                    stage3[3] = 10f;
                }
            }
        }
        else if (health <= 0)
        {
            QuestManager.instance.AliceIsDie = true;
            Destroy(gameObject);
        }
    }


    public void TakeDamage(int damage)      //受到伤害量
    {
        Debug.Log("TakeDamage");
        health -= damage;
    }
    public void GetHit(Vector2 direction,int damage)       // 受击
    {
        health -= damage;
        Debug.Log("Health = " + health);
        Debug.Log("GetHit");
        transform.localScale = new Vector3(-direction.x, 1, 1);
        isHit = true;
        collider2D.enabled = false;

        //TakeDamage(damage);
        Invoke("Invisible", invisibleTime);

        this.direction = direction;
        FlashColor(0.1f);
        hitAudio.Play();
        Debug.Log("66");
    }
    public void AttackOver()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    void OnTriggerEnter2D(Collider2D other)     //攻击判定
    {
        if (other.gameObject.tag == "Player")
        {
            if (FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
        }
    }

    public void FlashColor(float time)
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", time);
    }
    public void ResetColor()
    {
        spriteRenderer.color = O_color;
    }
    public void Hitover()
    {
        isHit = false;
    }
    void Invisible()        // 无敌时间
    {
        collider2D.enabled = true;
    }
    void FlipTo()
    {
        if (transform.position.x > target.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (transform.position.x < target.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void CandyAtk()
    {
        int i = Random.Range(1, 5);
        float xx, yy, zz;
        xx = gameObject.transform.position.x;
        yy = gameObject.transform.position.y;
        zz = gameObject.transform.position.z;
        if (i == 1) GameObject.Instantiate<GameObject>(atkCandy1, new Vector3(xx, yy + 5, zz), Quaternion.identity);
        if (i == 2) GameObject.Instantiate<GameObject>(atkCandy2, new Vector3(xx, yy + 5, zz), Quaternion.identity);
        if (i == 3) GameObject.Instantiate<GameObject>(atkCandy3, new Vector3(xx, yy + 5, zz), Quaternion.identity);
        if (i == 4) GameObject.Instantiate<GameObject>(atkCandy4, new Vector3(xx, yy + 5, zz), Quaternion.identity);
    }
    void HealCandy()
    {
        int i = Random.Range(1, 5);
        float xx, yy, zz;
        xx = gameObject.transform.position.x;
        yy = gameObject.transform.position.y;
        zz = gameObject.transform.position.z;
        if (i == 1) GameObject.Instantiate<GameObject>(healCandy1, new Vector3(xx, yy + 5, zz), Quaternion.identity);
        if (i == 2) GameObject.Instantiate<GameObject>(healCandy2, new Vector3(xx, yy + 5, zz), Quaternion.identity);
        if (i == 3) GameObject.Instantiate<GameObject>(healCandy3, new Vector3(xx, yy + 5, zz), Quaternion.identity);
        if (i == 4) GameObject.Instantiate<GameObject>(healCandy4, new Vector3(xx, yy + 5, zz), Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackArea);
    }
    
}
