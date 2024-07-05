using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellDog : NormalEnemy
{
    // Start is called before the first frame update
    protected override void Update()
    {
        animator = transform.GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if(!isDead)
            Move();
    }

    public override void GetHit(Vector2 direction)
    {
        hitAudio.Play();
        isHit = true;
        isAttack = false;
        rigidbody.velocity = direction * backSpeed;
        transform.localScale = new Vector3(-direction.x, 1, 1);
        this.direction = direction;
        
        if (healthPoint <= 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            isDead = true;
            Destroy(gameObject);
        }

    }
}
