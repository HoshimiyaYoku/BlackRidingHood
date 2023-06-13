using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private bool isEntered;
    private void Update() 
    {
        if (isEntered && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("111");
            FinalMovement.instance.health = FinalMovement.instance.maxHealth;
            FinalMovement.instance.magic = FinalMovement.instance.maxMagic;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            isEntered = true;
            // Debug.Log("111");
            //Debug.Log("Saved" + SceneManager.GetActiveScene());
            SaveManager.instance.scenePassword = SceneManager.GetActiveScene().name;
            SaveManager.instance.positionPassword = transform.position;

        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            isEntered = false;
        }    
    }
}
