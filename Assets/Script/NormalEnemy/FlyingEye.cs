using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : NormalEnemy
{
    public Transform wayPoint01, wayPoint02;
    private Transform wayPointTarget;
    private void Awake()
    {
        wayPointTarget = wayPoint01;
    }

    protected override void Move()
    {
        base.Move();

        if(!Physics2D.OverlapCircle(enemyCheckPoint.position, enemyCheckArea, targetLayer))
        {
            animator.SetBool("isMove", true);
            if(Vector2.Distance(transform.position, wayPoint01.position) < 1f)
            {
                wayPointTarget = wayPoint02;
            }

            if(Vector2.Distance(transform.position, wayPoint02.position) < 1f)
            {
                wayPointTarget = wayPoint01;
            }
            FlipTo(wayPointTarget);
            transform.position = Vector2.MoveTowards(transform.position, wayPointTarget.position, moveSpeed * Time.deltaTime);
        }
    }
}
