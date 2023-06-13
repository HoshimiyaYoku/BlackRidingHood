using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadRabbitControl : MonoBehaviour
{
    public GameObject deadWolf;
    private void Update() {
        if (QuestManager.instance.rabbitIsDie && GameObject.Find("Bunny") == null)
            deadWolf.SetActive(true);
   }
}
