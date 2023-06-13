using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFire : MonoBehaviour
{

    public GameObject werewolf;
    public GameObject movePoint;
    public GameObject spawnPoint;
    public float movespeed;
    private SpriteRenderer spriteRenderer;
    private bool isIns = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRenderer.color.a < 9 && !isIns)
        {
            Color a = spriteRenderer.color;
            //Debug.Log("a = " + spriteRenderer.color.a);
            a.a = a.a + 0.01f;
            spriteRenderer.color = a;
        }
        else
        {
            if (Vector2.Distance(gameObject.transform.position, movePoint.transform.position) > .1f && !isIns)
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,movePoint.transform.position, movespeed * Time.deltaTime);
        }

        if(Vector2.Distance(gameObject.transform.position,movePoint.transform.position) < .1f && !isIns)
        {
            GameObject.Instantiate(werewolf, spawnPoint.transform.position, spawnPoint.transform.rotation);
            isIns = true;
        }
        if (isIns)
        {
            Color a = spriteRenderer.color;
            a.a = a.a - 0.1f;
            spriteRenderer.color = a;
            if(spriteRenderer.color.a == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
