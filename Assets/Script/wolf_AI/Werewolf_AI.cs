using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Werewolf_AI : MonoBehaviour
{
    private Animator animator;
    private float idleTimer = 2f;
    //private GameObject Player;//追踪玩家
    //各种弹幕模式，我懒得写复杂的了
    public GameObject Liner;//基础自机狙
    public GameObject fiveWays;//5路返回子弹
    public GameObject elevenWays;//11路子弹
    public GameObject randomAiming;//随机散射追踪
    public GameObject Homing;//大招月球
    public GameObject RandomSpiralMulti;//大范围圆形随机
    public GameObject ThirtyWayAllRange;//极大范围圆形
    public float stateTimer;//状态切换时间
    public float health;
    private float startHealth;
    private int state;//弹幕状态指示器
    private float timer;
    private bool stateoneClear = true;
    private bool statetwoClear = true;
    private bool deadanimation = false;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        timer = stateTimer;
        target = GameObject.Find("Player").transform;
        FlipTo();
        animator = GetComponent<Animator>();
        startHealth = health;
        state = Random.Range(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (idleTimer >= 0)
        {
            idleTimer -= Time.deltaTime;
        }
        else if (health > 0)
        {   FlipTo();
            if (health  >= 150f)
            {

                if (timer <= 0)
                {
                    timer = stateTimer;
                    state = Random.Range(0, 10);
                }
                //Debug.Log("a = " + state);
                if (state >= 0 && state < 5 && timer >= 0)
                {
                    animator.SetTrigger("Atk1");
                    Liner.SetActive(true);
                    //Debug.Log("Liner");
                    //Debug.Log("timer = " + timer);
                    timer -= Time.deltaTime;
                }
                else
                {
                    Liner.SetActive(false);

                }

                if (state >= 5 && state < 8 && timer >= 0)
                {
                    animator.SetTrigger("Atk1");
                    randomAiming.SetActive(true);
                    timer -= Time.deltaTime;
                }
                else
                {
                    randomAiming.SetActive(false);
                }

                if (state >= 8 && state < 10 && timer >= 0)
                {
                    animator.SetTrigger("Atk2");
                    elevenWays.SetActive(true);
                    timer -= Time.deltaTime;
                }
                else
                {
                    elevenWays.SetActive(false);
                }

            }
            else if (health  >= 50f && health < 150f)
            {
                if (timer <= 0)
                {
                    timer = stateTimer;
                    state = Random.Range(0, 10);
                }
                if (stateoneClear)//清空一阶段弹幕状态
                {
                    Liner.SetActive(false);
                    randomAiming.SetActive(false);
                    elevenWays.SetActive(false);
                    stateoneClear = false;
                    state = Random.Range(0, 10);
                    timer = stateTimer;
                }

                if (state >= 0 && state < 3 && timer >= 0)
                {
                    animator.SetTrigger("Atk1");
                    fiveWays.SetActive(true);
                    timer -= Time.deltaTime;
                }
                else
                {
                    fiveWays.SetActive(false);
                }

                if (state >= 3 && state < 5 && timer >= 0)
                {
                    animator.SetTrigger("Atk1");
                    randomAiming.SetActive(true);
                    timer -= Time.deltaTime;
                }
                else
                {
                    randomAiming.SetActive(false);
                }

                if (state >= 5 && state < 8 && timer >= 0)
                {
                    animator.SetTrigger("Atk2");
                    ThirtyWayAllRange.SetActive(true);
                    timer -= Time.deltaTime;
                }
                else
                {
                    ThirtyWayAllRange.SetActive(false);
                }

                if (state >= 8 && state < 10 && timer >= 0)
                {
                    animator.SetTrigger("Atk2");
                    RandomSpiralMulti.SetActive(true);
                    timer -= Time.deltaTime;
                }
                else
                {
                    RandomSpiralMulti.SetActive(false);
                }


            }
            else if (health  > 0 && health < 50f)
            {
                if (statetwoClear)//清空二阶段弹幕状态
                {
                    fiveWays.SetActive(false);
                    randomAiming.SetActive(false);
                    ThirtyWayAllRange.SetActive(false);
                    RandomSpiralMulti.SetActive(false);
                    statetwoClear = false;
                    timer = stateTimer;
                }
                animator.SetTrigger("Atk1");
                if (Homing.activeInHierarchy != true) Homing.SetActive(true);
                if (randomAiming.activeInHierarchy != true) randomAiming.SetActive(true);

            }
            else
            {
                fiveWays.SetActive(false);
                randomAiming.SetActive(false);
                ThirtyWayAllRange.SetActive(false);
                RandomSpiralMulti.SetActive(false);
                Homing.SetActive(false);
                Liner.SetActive(false);

                elevenWays.SetActive(false);
            }
        }
        if(health <= 0)
        {
            
            animator.ResetTrigger("Atk1");
            animator.ResetTrigger("Atk2");
            animator.ResetTrigger("Hit");
            if (!deadanimation)
            {
                animator.SetTrigger("Dead");
                deadanimation = true;
            }
            
            fiveWays.SetActive(false);
            randomAiming.SetActive(false);
            ThirtyWayAllRange.SetActive(false);
            RandomSpiralMulti.SetActive(false);
            Homing.SetActive(false);
            Liner.SetActive(false);
            elevenWays.SetActive(false);
            QuestManager.instance.wolfIsDie = true;
        }

    }
    void FlipTo()
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
