using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCandy : MonoBehaviour
{
    public int damage;
    public float arrawDistance;
    private FinalMovement playerHealth;
    private Rigidbody2D rg2d;
    private Vector3 startPos;
    private float timer = 8.0f;
    private float x, y, z;
    private bool add = false;

    // Use this for initialization
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalMovement>();
        rg2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        x = Random.Range(-4f, 4f);
        y = Random.Range(9.8f, 15f);
        z = Random.Range(-10f, 10f);
        
    }

    void Update()
    {
        if (!add)
        {
            addforce();
            rg2d.AddTorque(z);
            add = true;
        }
        
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
    }
    void addforce()
    {
        rg2d.AddForce(new Vector3(x, y, z), ForceMode2D.Impulse);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (FinalMovement.instance != null)
            {
                FinalMovement.instance.DamagePlayer(damage);
            }
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "PlayerBullet")
        {

            Destroy(gameObject);

        }
    }
}
