using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : Mushroom
{
    protected override void Update()
    {
        base.Update();
        if (healthPoint <= 0  && !isDead)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            isDead = true;
            animator.SetTrigger("Die");
        }
    }
    private void Boomer()
    {
        healthPoint = 0;
    }
    public override void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player") 
        {
            if(FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
        }
    }
}
