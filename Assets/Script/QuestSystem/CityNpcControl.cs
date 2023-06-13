using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityNpcControl : MonoBehaviour
{
    private void update() {
        Debug.Log("111");
        if (QuestManager.instance.wolfIsDie && QuestManager.instance.rabbitIsDie)
        {
            Debug.Log("111");
            GameObject.Find("Alice 1").SetActive(false);
        }
    }
}
