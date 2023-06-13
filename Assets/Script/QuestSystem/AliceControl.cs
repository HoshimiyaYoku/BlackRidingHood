using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceControl : MonoBehaviour
{
    public GameObject deadWolf;
    private void Update() {
        if (GameObject.Find("alice_boss") == null)
            deadWolf.SetActive(true);
    }
}
