using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPlus : MonoBehaviour
{
    public bool isUsed;
    public int bottleNumber;
    private Transform playerTrans;
    [SerializeField] private int plusMaxHealth;
    private void Start() {
        if (QuestManager.instance.bloodControl[bottleNumber] == 1)
            gameObject.SetActive(false);
    }
    private void Update() 
    {
        if (isUsed)
            gameObject.SetActive(false);   
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            isUsed = true;
            FinalMovement.instance.maxHealth += plusMaxHealth;
            FinalMovement.instance.health += plusMaxHealth;
            gameObject.SetActive(false);
            QuestManager.instance.bloodControl[bottleNumber] = 1;
        }    
    }
}
