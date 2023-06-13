using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateTpye
{
    Idle,Chase,Attack,Hit,Death
}
[Serializable]
public class Parameter//敌人的各种状态属性
{
    public int health;

    //public float moveSpeed;
    public float chaseSpeed;

    public float idleTime;

    public Transform[] chasePoints;

    public Animator animator;

    public Transform target;

    public LayerMask targetLayer;

    public Transform attackPoint;

    public float attackArea;

    public GameObject blackfire;
}

public class FSM : MonoBehaviour
{
    public Parameter parameter;
    public bool isHit = false;
    private IState currentState;//当前状态
    private Dictionary<StateTpye, IState> states = new Dictionary<StateTpye, IState>();

    // Start is called before the first frame update
    void Start()
    {
        parameter.target = GameObject.Find("Player").transform;
        states.Add(StateTpye.Idle, new IdleState(this));//添加待机状态
        states.Add(StateTpye.Chase, new ChaseState(this));//添加追击状态
        states.Add(StateTpye.Attack, new AttackState(this));//添加攻击状态
        states.Add(StateTpye.Hit, new HitState(this));//添加受击状态
        states.Add(StateTpye.Death, new DeathState(this));//添加死亡状态
        TransitionState(StateTpye.Idle);

        parameter.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
        if (isHit)
        {
            //Debug.Log("isHit");
            TransitionState(StateTpye.Hit);
            isHit = false;
        }
    }


    public void TransitionState(StateTpye type)//状态切换
    {
        if(currentState != null) currentState.OnExit();//如果当前状态不为空，则退出前一个状态
        currentState = states[type];//通过字典枚举找到状态类型
        currentState.OnEnter();//执行新的状态的进入函数
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);

    }
}
