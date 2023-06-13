using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : NormalEnemy
{
    protected override void Move()
    {
        
    }

    protected override void Attack()
    {
        
    }
    protected override void Update()
    {
        base.Update();
        if (!isDead)
            FlipTo(target);
        if (QuestManager.instance.rabbitIsDie)
            gameObject.SetActive(false);
    }
    public override void GetHit(Vector2 direction)
    {
        hitAudio.Play();
        hitAnimator.SetTrigger("Hit");
        isHit = true;
        isAttack = false;
        rigidbody.velocity = direction * backSpeed;
        transform.localScale = new Vector3(-direction.x, 1, 1);
        this.direction = direction;
        
        if (healthPoint <= 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            isDead = true;
            animator.SetTrigger("Die");
            this.transform.tag = "Untagged";
        }

    }

    protected override void DeathTime()
    {
        QuestManager.instance.rabbitIsDie = true;
        FinalMovement.instance.doubleJump = true;
    }
}
