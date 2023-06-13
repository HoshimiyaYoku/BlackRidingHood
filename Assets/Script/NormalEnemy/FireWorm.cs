using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : Mushroom
{
    private float shotRate = 2.0f;
    private float shotTimer;
    public GameObject projectile;
    public Transform shotArea;
    private bool imAt = true;
    protected override void Attack()
    {
        RemoteAttack();
    }

    private void RemoteAttack()
    {
        if (!Physics2D.OverlapCircle(attackPoint.position, attackArea, targetLayer))
            imAt = true;
        shotTimer += Time.deltaTime;
        if (Physics2D.OverlapCircle(attackPoint.position, attackArea, targetLayer) && imAt)
        {
            FlipTo(target);
            animator.SetBool("isMove", false);
            isAttack = true;
            animator.SetBool("isAttack", true);
            imAt = false;
            shotTimer = 0;
        }
        else if (Physics2D.OverlapCircle(attackPoint.position, attackArea, targetLayer) && shotTimer >= shotRate)
        {
            FlipTo(target);
            animator.SetBool("isMove", false);
            isAttack = true;
            animator.SetBool("isAttack", true);
            shotTimer = 0;
        }
    }

    private void shot()
    {
        Instantiate(projectile, shotArea.position, Quaternion.identity);
    }
}
