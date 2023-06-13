using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControl1 : MonoBehaviour
{
    private void Update() 
    {
        if (GameObject.Find("alice_boss") == null)
        {
            gameObject.SetActive(false);
        }
    }
}
