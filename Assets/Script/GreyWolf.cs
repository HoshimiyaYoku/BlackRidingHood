using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyWolf : Enemy
{
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
        if (QuestManager.instance.wolfIsDie && GameObject.Find("g_wolf") != null)
            Destroy(gameObject);
    }
}
