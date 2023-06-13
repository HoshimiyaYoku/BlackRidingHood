using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMovement : MonoBehaviour
{
    public static FinalMovement instance;
    public Rigidbody2D rb;
    public Collider2D coll;
    public Animator anim;
    public float speed, jumpForce;
    public int damage, health, maxHealth, maxMagic, magic;
    public Transform groundCheck;
    public float groundCheckArea;
    public LayerMask ground;
    public GameObject Necklace;
    public GameObject weapon;

    public bool isGround, isJump;
    public bool doubleJump;
    int skyAttack, jumpCount;
    public bool isAttack;
    public bool canAttack = true;
    public bool isChange = false;//小红帽兽化
    public bool canMove = true;
    public bool isShoot;
    public bool isHurt;
    public bool isAlive;
    public bool isTalk;


    public float timer;
    public float changeTimer = 10.0f;
    public float changeCD = 0f;
    public CircleCollider2D hitBox;
	public float invisibleTime;
    public AudioSource hitAudio;
    public AudioSource waveAudio;
    public AudioSource jumpAudio;
    public AudioSource gunAudio;
    private Scene scene;

    public string scenePassword;

    public GameObject PlayerUI;

    // Start is called before the first frame update
    private void Awake() 
    {
        scene = SceneManager.GetActiveScene();
        
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        magic = maxMagic;
        health = maxHealth;

        isAlive = true;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Necklace = GameObject.Find("Necklace").gameObject;
        weapon = GameObject.Find("weapon_axe").gameObject;
        hitBox = GameObject.Find("CheckPoint").gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log(scene.name);
      
        
        if(magic < 0)
            magic = 0;
        if(magic > maxMagic)
            magic = maxMagic; 
        
        if(health < 0)
            health = 0;
        if(health > maxHealth)
            health = maxHealth;

        if (isChange)
        {
            changeTimer -= Time.deltaTime;
            //Debug.Log("changeTimer = " + changeTimer);
            if(changeTimer <= 0f && isGround && isAlive && !isAttack && isChange)
            {
                DisChange();
            }
        }
        else
        {
            //Debug.Log("changeCD = " + changeCD);
            if (changeCD < 10.0f)
            {
                changeCD += Time.deltaTime;
            }
            if(changeCD >= 10.0f)
            {
                changeCD = 10.0f;
            }
        }

        if (!isTalk && GameObject.Find("Pausemenu") == null)
        {
            if(changeCD >= 10.0f&& !isChange)//CD好了才能变身
            {
                Change();
            }
            
            Attack();
            Jump();
            Shoot();
        }
        if (!isAlive && GameObject.Find("Pausemenu") == null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
    }

    private void FixedUpdate()
    {

        GroundMovement();
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckArea, ground);
             //判断是否在地面

        SwitchAnim();
        
        
    }

    void GroundMovement()   //移动
    {
        if (canMove)
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            if (!isAttack && !isHurt && isAlive && !isShoot && !isTalk)
                rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
            if (horizontalMove != 0 && !isAttack && !isHurt && isAlive && !isTalk && !isShoot)
            {
                transform.localScale = new Vector3(horizontalMove, 1, 1);
            }
        }
    }

    void Jump()     // 跳跃
    {
        if(isGround && isAlive)
        {
            if(!doubleJump)
                jumpCount = 1;
            else if(doubleJump)
                jumpCount = 2;
            isJump = false;
        }
        
        if(!isGround && !isJump)        //修复在没有二段跳的情况下可以落下跳跃的bug
        {
            if(Input.GetButtonDown("Jump") && !isHurt && isAlive && doubleJump)
            {
                jumpAudio.Play();
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount -= 2;
            }
        }
        
        if(Input.GetButtonDown("Jump") && isGround && !isHurt && isAlive)
        {
            jumpAudio.Play();
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
        }
        else if(Input.GetButtonDown("Jump") && jumpCount > 0 && !isGround && isJump && !isHurt && isAlive)
        {
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount --;
        }
    }

    void SwitchAnim()   //动画相关
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if(isGround)
        {
            anim.SetBool("Fall", false);
            anim.SetBool("Jump", false);
        }
        else if(!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("Jump", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
        }
        if(isHurt)
        {
            anim.SetBool("isHurt", true);
            if(Mathf.Abs(rb.velocity.x) < 0.2f)
            {
                isHurt = false;
                anim.SetBool("isHurt", false);
                coll.sharedMaterial = new PhysicsMaterial2D()
                    {
                        friction = 0.0f, bounciness = 0.0f
                    };
            }
        }
    }
    void Attack() //角色攻击开始
    {
        if(isGround)
            skyAttack = 1;
        if (Input.GetButtonDown("Attack") && !isAttack && skyAttack > 0 && !isHurt && isAlive && canAttack && !isShoot)
        {
            waveAudio.Play();
            Necklace.SetActive(false);
            anim.SetBool("isAttack", true);
            isAttack = true;
            skyAttack--;
            if(isGround)
                rb.velocity = new Vector2(rb.velocity.x*0.1f, rb.velocity.y);
            else if(!isGround)
                rb.velocity = new Vector2(rb.velocity.x*0.3f, rb.velocity.y);
            anim.SetTrigger("Attack");
        }
    }

    public void AttackOver()    //攻击结束
    {
        isAttack = false;
        anim.SetBool("isAttack", false);
        Necklace.SetActive(true);
    }

    // private void OnTriggerEnter2D(Collider2D other)      // 受击判定
    // {
        
       
    // }
    private void OnTriggerEnter2D(Collider2D other)     // 攻击触发判定and受击判定
    {
        if((other.gameObject.tag == "Enemy") && isAttack && isAlive)      //攻击
        {
            magic += 2;
            AttackScene.Instance.HitPause(12);//原摄像机时停(传入帧数)
            AttackScene.Instance.CameraShake(0.1f, 0.08f);//原摄像机抖动(时间，抖动力度)
            //CinemachineShake.Instance.ShakeCamera(10f, .2f);
            
            if (transform.localScale.x > 0)
            {
                if(other.GetComponent<Enemy>())
                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                else if(other.GetComponent<NormalEnemy>())
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.right);
            }
            else if (transform.localScale.x < 0)
            {
                if(other.GetComponent<Enemy>())
                    other.GetComponent<Enemy>().GetHit(Vector2.left);
                else if(other.GetComponent<NormalEnemy>())
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.left);
            }

            if (other.GetComponent<FSM>())//小狼专用补丁代码
            {
                other.GetComponent<FSM>().isHit = true;
            }
            if (other.GetComponent<Werewolf_AI>())//大狼专用补丁代码，用于检测血量
            {
                other.GetComponent<Werewolf_AI>().health -= damage;
            }

            if(other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(damage);
            else if(other.GetComponent<NormalEnemy>())
                other.GetComponent<NormalEnemy>().TakeDamage(damage);
            
            if(transform.position.x < other.gameObject.transform.position.x)    //后坐力
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
            }
            else if(transform.position.x > other.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
            }
        }
        
        else if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyAttack")  && isAlive)   //受到伤害后退
        {
            if (!isChange)
            {
                hitAudio.Play();
                AttackScene.Instance.CameraShake(0.1f, 0.1f);//原摄像机抖动(时间，抖动力度)
                coll.sharedMaterial = new PhysicsMaterial2D()
                {
                    friction = 0.3f,
                    bounciness = 0.0f
                };
                if (anim.GetBool("Fall"))
                {
                    rb.velocity = new Vector2(rb.velocity.x, 10);
                    isHurt = true;
                }
                if (transform.position.x < other.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(-9, 12);
                    isHurt = true;
                }
                else if (transform.position.x > other.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(9, 12);
                    isHurt = true;
                }
            }
            else
            {
                hitAudio.Play();
                AttackScene.Instance.CameraShake(0.1f, 0.1f);//原摄像机抖动(时间，抖动力度)
                isHurt = true;
                coll.sharedMaterial = new PhysicsMaterial2D()
                {
                    friction = 0.3f,
                    bounciness = 0.0f
                };
            }
        }
    }
    public void DamagePlayer(int damageGet)     //伤害量判定
    {
        if (isChange)
        {
            health = health - (int)(damageGet * 0.5f);//减伤代码，按照需要再调整平衡吧
        }
        else
        {
            health -= damageGet;
        }
        
        //无敌时间
        
        
        if(health < 0)
            health = 0;
        
        if (health <= 0)
        {
            Necklace.SetActive(false);
            weapon.SetActive(false);
            hitBox.enabled = false;
            isAlive = false;
            anim.SetTrigger("Die");
            // rb.velocity = new Vector2(0, -10);
            PlayerUI.GetComponent<GameOver>().Setup();
        }
        else
        {
            hitBox.enabled = false;
            Invoke("Invisible", invisibleTime);
        }
    }
    void Invisible()        // 无敌时间
    {
        hitBox.enabled = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckArea);
    }

    public void Shoot()
    {
        if(Input.GetButtonDown("Fire2") && isAlive && !isAttack && magic >= 6 && !isChange)
        {
            Necklace.SetActive(false);
            if (isGround)
                rb.velocity = new Vector2(rb.velocity.x*0.1f, rb.velocity.y);
            else if(!isGround)
                rb.velocity = new Vector2(rb.velocity.x*0.3f, rb.velocity.y);
            anim.SetBool("isShoot", true);
            isShoot = true;
            anim.SetTrigger("Shoot");
            magic -= 6;          
        }
    }

    


    public void ShootOver()    //攻击结束
    {
        isShoot = false;
        anim.SetBool("isShoot", false);
        Necklace.SetActive(true);
    }

    public void AnimShoot()
    {
        gunAudio.Play();
        GameObject.Find("Gun").GetComponent<Gun>().Shoot();
    }


    public void Change()//HENSHIN!
    {
        if (Input.GetButtonDown("Fire3") && isAlive && !isAttack && !isChange && isGround)
        {
            damage = damage + 5;
            changeCD = 0;
            rb.velocity = new Vector2(rb.velocity.x * 0f, rb.velocity.y);
            Necklace.SetActive(false);
            canMove = false;
            anim.SetTrigger("Change");
            anim.SetBool("isChange", true);
            isChange = true;
        }
    }

    public void ChangingOver()    //攻击结束
    {
        //Debug.Log("ChangingOver");
        canMove = true;
        Necklace.SetActive(true);
        //Debug.Log("canMove = " + canMove);
        changeTimer = 10.0f;
    }

    public void DisChange()//解除变身
    {
        changeTimer = 10.0f;
        //Debug.Log("DisChange");
        anim.SetTrigger("disChange");
        Necklace.SetActive(false);
        rb.velocity = new Vector2(rb.velocity.x * 0f, rb.velocity.y);
        canMove = false;
    }

    public void DisChangeOver()
    {
        canMove = true;
        Necklace.SetActive(true);
        isChange = false;
        anim.SetBool("isChange", false);
        //Debug.Log("DisChangeOver");
        changeTimer = 10.0f;
        damage = damage - 5;
    }
}  

