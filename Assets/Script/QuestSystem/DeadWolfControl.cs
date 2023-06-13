using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadWolfControl : MonoBehaviour
{
   public GameObject deadWolf;
   private void Update() {
        if (QuestManager.instance.wolfIsDie && GameObject.Find("b_werewolf(Clone)") == null)
            deadWolf.SetActive(true);
   }
}
