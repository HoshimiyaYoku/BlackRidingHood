using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.instance.wolfIsDie && QuestManager.instance.rabbitIsDie)
            Destroy(GameObject.Find("Alice 1"));
    }
}
