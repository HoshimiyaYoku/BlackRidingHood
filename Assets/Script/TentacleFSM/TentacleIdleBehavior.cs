using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleIdleBehavior : StateMachineBehaviour
{
    public float attackRate = 4.0f;
    public float attackTimer;

    public GameObject enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        enemy.transform.GetChild(0).transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        attackTimer = attackRate;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            //animator.SetBool("isAttack", true);
            animator.SetTrigger("isAttacked");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
