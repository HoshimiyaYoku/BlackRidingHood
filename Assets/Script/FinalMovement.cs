using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMovement : MonoBehaviour
{
    // Singleton instance of the FinalMovement class
    public static FinalMovement instance;
    public Rigidbody2D rb;
    public Collider2D coll;
    public Animator anim;
    
    // Player movement and status attributes
    public float speed, jumpForce;
    public int damage, health, maxHealth, maxMagic, magic;
    public Transform groundCheck;
    public float groundCheckArea;
    public LayerMask ground;
    public GameObject Necklace;
    public GameObject weapon;

    // Player state flags
    public bool isGround, isJump;
    public bool doubleJump;
    int skyAttack, jumpCount;
    public bool isAttack;
    public bool canAttack = true;
    public bool isChange = false; // Red Riding Hood transformation
    public bool canMove = true;
    public bool isShoot;
    public bool isHurt;
    public bool isAlive;
    public bool isTalk;
    public bool isHenshin;

    // Transformation and invincibility timers
    public float timer;
    public float changeTimer = 10.0f;
    public float changeCD = 0f;
    public CircleCollider2D hitBox;
    public float invisibleTime;
    
    // Audio sources for various actions
    public AudioSource hitAudio;
    public AudioSource waveAudio;
    public AudioSource jumpAudio;
    public AudioSource gunAudio;
    
    // Current scene
    private Scene scene;

    // Password for the current scene
    public string scenePassword;

    // Reference to the player UI
    public GameObject PlayerUI;

    // Called before the first frame update
    private void Awake() 
    {
        // Get the active scene
        scene = SceneManager.GetActiveScene();
        
        // Implement singleton pattern
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

    // Called before the first frame update
    void Start()
    {
        // Initialize player stats
        magic = maxMagic;
        health = maxHealth;
        isAlive = true;

        // Get necessary components
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Necklace = GameObject.Find("Necklace").gameObject;
        weapon = GameObject.Find("weapon_axe").gameObject;
        hitBox = GameObject.Find("CheckPoint").gameObject.GetComponent<CircleCollider2D>();
    }

    // Called once per frame
    void Update()
    {
        // Heal player if certain conditions are met
        if (health < maxHealth && magic >= 6 && Input.GetKeyDown(KeyCode.V))
        {
            health += (int)(0.3 * maxHealth);
            magic -= 6;
        }
        
        // Clamp magic and health values to their maximum
        if(magic < 0)
            magic = 0;
        if(magic > maxMagic)
            magic = maxMagic; 
        if(health < 0)
            health = 0;
        if(health > maxHealth)
            health = maxHealth;

        // Handle transformation logic
        if (isChange)
        {
            changeTimer -= Time.deltaTime;
            if(changeTimer <= 0f && isGround && isAlive && !isAttack && isChange)
            {
                DisChange();
            }
        }
        else
        {
            if (changeCD < 30.0f)
            {
                changeCD += Time.deltaTime;
            }
            if(changeCD >= 30.0f)
            {
                changeCD = 30.0f;
            }
        }

        // Handle player actions if not talking and game is not paused
        if (!isTalk && GameObject.Find("Pausemenu") == null)
        {
            Attack();
            Jump();
            Shoot();
            if (changeCD >= 10.0f && !isChange && QuestManager.instance.wolfIsDie) // Can transform only if cooldown is ready
            {
                Change();
            }
        }

        // Stop player movement if dead
        if (!isAlive && GameObject.Find("Pausemenu") == null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    // Called at fixed time intervals
    private void FixedUpdate()
    {
        GroundMovement();
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckArea, ground); // Check if on the ground
        SwitchAnim();
    }

    // Handles player movement
    void GroundMovement()
    {
        if (canMove)
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            if (!isAttack && !isHurt && isAlive && !isShoot && !isTalk && isAlive)
                rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
            if (horizontalMove != 0 && !isAttack && !isHurt && isAlive && !isTalk && !isShoot && isAlive)
            {
                transform.localScale = new Vector3(horizontalMove, 1, 1);
            }
        }
    }

    // Handles player jumping
    void Jump()
    {
        if(isGround && isAlive)
        {
            if(!doubleJump)
                jumpCount = 1;
            else if(doubleJump)
                jumpCount = 2;
            isJump = false;
        }
        
        // Fix bug allowing jump while falling without double jump
        if(!isGround && !isJump)
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

    // Switches player animations based on state
    void SwitchAnim()
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

    // Start character attack
    void Attack()
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

    // End attack
    public void AttackOver()
    {
        isAttack = false;
        anim.SetBool("isAttack", false);
        Necklace.SetActive(true);
    }

    // Attack and hit detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if((other.gameObject.tag == "Enemy") && isAttack && isAlive) // Attack
        {
            magic += 2;
            AttackScene.Instance.HitPause(12); // Original camera pause (frames)
            AttackScene.Instance.CameraShake(0.1f, 0.08f); // Original camera shake (duration, intensity)

            if (transform.localScale.x > 0)
            {
                if (other.GetComponent<Enemy>())
                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                else if (other.GetComponent<NormalEnemy>())
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.right);
                else if (other.GetComponent<Alice_AI>())
                    other.GetComponent<Alice_AI>().GetHit(Vector2.right, damage);
            }
            else if (transform.localScale.x < 0)
            {
                if(other.GetComponent<Enemy>())
                    other.GetComponent<Enemy>().GetHit(Vector2.left);
                else if(other.GetComponent<NormalEnemy>())
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.left);
                else if (other.GetComponent<Alice_AI>())
                    other.GetComponent<Alice_AI>().GetHit(Vector2.left, damage);
            }

            if (other.GetComponent<FSM>()) // Small wolf specific patch
            {
                other.GetComponent<FSM>().isHit = true;
            }
            if (other.GetComponent<Werewolf_AI>()) // Large wolf specific patch, for health detection
            {
                other.GetComponent<Werewolf_AI>().health -= damage;
            }

            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(damage);
            else if (other.GetComponent<NormalEnemy>())
                other.GetComponent<NormalEnemy>().TakeDamage(damage);

            if (!isHenshin)
                if(transform.position.x < other.gameObject.transform.position.x) // Recoil
                {
                    rb.velocity = new Vector2(-5, rb.velocity.y);
                }
                else if(transform.position.x > other.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(5, rb.velocity.y);
                }
        }
        else if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyAttack") && isAlive) // Knockback on hit
        {
            if (!isChange)
            {
                hitAudio.Play();
                AttackScene.Instance.CameraShake(0.1f, 0.1f); // Original camera shake (duration, intensity)
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
                AttackScene.Instance.CameraShake(0.1f, 0.1f); // Original camera shake (duration, intensity)
                isHurt = true;
                coll.sharedMaterial = new PhysicsMaterial2D()
                {
                    friction = 0.3f,
                    bounciness = 0.0f
                };
            }
        }
    }

    // Damage calculation
    public void DamagePlayer(int damageGet)
    {
        if (isChange)
        {
            health = health - (int)(damageGet * 0.5f); // Damage reduction code, adjust balance as needed
        }
        else
        {
            health -= damageGet;
        }

        // Invincibility time
        if(health < 0)
            health = 0;
        
        if (health <= 0)
        {
            Necklace.SetActive(false);
            weapon.SetActive(false);
            hitBox.enabled = false;
            isAlive = false;
            anim.SetTrigger("Die");
            PlayerUI.GetComponent<GameOver>().Setup();
        }
        else
        {
            hitBox.enabled = false;
            Invoke("Invisible", invisibleTime);
        }
    }

    // Invincibility period
    void Invisible()
    {
        hitBox.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckArea);
    }

    // Shoot action
    public void Shoot()
    {
        if(Input.GetButtonDown("Fire2") && isAlive && !isAttack && magic >= 3 && !isChange)
        {
            Necklace.SetActive(false);
            if (isGround)
                rb.velocity = new Vector2(rb.velocity.x*0.1f, rb.velocity.y);
            else if(!isGround)
                rb.velocity = new Vector2(rb.velocity.x*0.3f, rb.velocity.y);
            anim.SetBool("isShoot", true);
            isShoot = true;
            anim.SetTrigger("Shoot");
            magic -= 3;          
        }
    }

    // End shoot
    public void ShootOver()
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

    // Transformation (HENSHIN!)
    public void Change()
    {
        if (Input.GetButtonDown("Fire3") && isAlive && !isAttack && !isChange && isGround)
        {
            isChange = true;
            isHenshin = true;
            damage = damage + 5;
            changeCD = 0;
            rb.velocity = new Vector2(rb.velocity.x * 0f, rb.velocity.y);
            Necklace.SetActive(false);
            canMove = false;
            anim.SetTrigger("Change");
            anim.SetBool("isChange", true);
        }
    }

    // End transformation
    public void ChangingOver()
    {
        canMove = true;
        Necklace.SetActive(true);
        changeTimer = 10.0f;
    }

    // End transformation
    public void DisChange()
    {
        changeTimer = 10.0f;
        anim.SetTrigger("disChange");
        Necklace.SetActive(false);
        rb.velocity = new Vector2(rb.velocity.x * 0f, rb.velocity.y);
        canMove = false;
    }

    // End transformation
    public void DisChangeOver()
    {
        canMove = true;
        Necklace.SetActive(true);
        isChange = false;
        anim.SetBool("isChange", false);
        isHenshin = false;
        changeTimer = 10.0f;
        damage = damage - 5;
    }
}


