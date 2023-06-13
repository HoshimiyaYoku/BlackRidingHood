using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControl : MonoBehaviour
{
    private void Update() 
    {
        if (QuestManager.instance.wolfIsDie)
        {
            gameObject.SetActive(false);
        }
    }
}
