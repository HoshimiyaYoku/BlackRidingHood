using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage;
    public float Speed;
    public float arrawDistance;

    private Rigidbody2D rg2d;
    private Vector3 startPos;
    private Transform playerTrans;//Weapon Return Position
    private float throwMoment;
    // Use this for initialization
    void Start()
    {
        playerTrans = FinalMovement.instance.transform;
        throwMoment = playerTrans.localScale.x;
        rg2d = GetComponent<Rigidbody2D>();
        if (throwMoment == 1)
        { 
            rg2d.velocity = transform.right * Speed; 
        }
        else if (throwMoment == -1)
        { 
            rg2d.velocity = transform.right * -Speed;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        startPos = transform.position;
    }

    void Update()
    {
        float distance = (transform.position - startPos).sqrMagnitude;
        if (distance > arrawDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")//FIXME isDamage [Avoid The AXE stop on the ground but still takes damage to Enemy]
        {
            AttackScene.Instance.CameraShake(0.1f, 0.1f);//原摄像机抖动(时间，抖动力度)
            if(other.GetComponent<Enemy>())
            {
                if (throwMoment > 0)
                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                else if (throwMoment < 0)
                    other.GetComponent<Enemy>().GetHit(Vector2.left);
            }
            else if(other.GetComponent<NormalEnemy>())
            {
                if (throwMoment > 0)
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.right);
                else if (throwMoment < 0)
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.left);
            }
            else if (other.GetComponent<Alice_AI>())
            {
                if (throwMoment > 0)
                    other.GetComponent<Alice_AI>().GetHit(Vector2.right, damage);
                else if (throwMoment < 0)
                    other.GetComponent<Alice_AI>().GetHit(Vector2.left, damage);
            }

            if (other.GetComponent<FSM>())//小狼专用补丁代码
            {
                other.GetComponent<FSM>().isHit = true;
            }
            if (other.GetComponent<Werewolf_AI>())//大狼专用补丁代码，用于检测血量
            {
                other.GetComponent<Werewolf_AI>().health -= damage;
            }

            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(damage);
            else if(other.GetComponent<NormalEnemy>())
                other.GetComponent<NormalEnemy>().TakeDamage(damage);
            
            Destroy(gameObject);    
        }
    }
}
