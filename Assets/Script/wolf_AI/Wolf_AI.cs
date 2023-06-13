using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState//待机状态
{
    private FSM manager;
    private Parameter parameter;
    private float timer;
    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()//进入状态
    {
        parameter.animator.Play("g_wolf_ide");
    }

    public void OnUpdate()//更新状态
    {
        timer += Time.deltaTime;
        if (timer >= parameter.idleTime)
        {
            manager.TransitionState(StateTpye.Chase);
        }
            
    }

    public void OnExit()//退出状态
    {
        timer = 0;
    }
}

public class ChaseState : IState//追击状态
{
    private FSM manager;
    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()//进入状态
    {
        parameter.animator.Play("g_wolf_run");
    }

    public void OnUpdate()//更新状态
    {
        manager.FlipTo(parameter.target);
        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
        parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            manager.TransitionState(StateTpye.Attack);
        }
    }

    public void OnExit()//退出状态
    {

    }
}

public class AttackState : IState//攻击状态
{
    private FSM manager;
    private Parameter parameter;

    private AnimatorStateInfo info;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()//进入状态
    {
        parameter.animator.Play("g_wolf_claw");
    }

    public void OnUpdate()//更新状态
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        if(info.normalizedTime >= .95f)
        {
            manager.TransitionState(StateTpye.Chase);
        }
    }

    public void OnExit()//退出状态
    {

    }
}

public class HitState : IState//受击状态
{
    private FSM manager;
    private Parameter parameter;

    public HitState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()//进入状态
    {
        parameter.health = parameter.health - 5;
        //Debug.Log("Health = ");
        //Debug.Log(parameter.health);
    }

    public void OnUpdate()//更新状态
    {
        if(parameter.health <= 0)
        {
            manager.TransitionState(StateTpye.Death);
        }
        else
        {
            manager.TransitionState(StateTpye.Chase);
        }
    }

    public void OnExit()//退出状态
    {

    }
}

public class DeathState : IState//死亡状态
{
    private FSM manager;
    private Parameter parameter;
    private bool canInst = true;
    private float timer = 2f;
    private AnimatorStateInfo info;

    public DeathState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()//进入状态
    {
        parameter.animator.Play("g_wolf_dead");
    }

    public void OnUpdate()//更新状态
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer < 0 && canInst)//生成二阶段火焰
            {
                GameObject wf = parameter.blackfire;
                Transform transform = manager.transform;
                Vector3 pos = transform.position;
                pos.y -= 0.5f;
                pos.x += 0.1f;
                transform.position = pos;
                GameObject.Instantiate(wf, transform.position, transform.rotation);
                canInst = false;
            }


    }

    public void OnExit()//退出状态
    {

    }
}

