using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    // Reference to the player GameObject
    public GameObject player;

    // Flag to check if the player has entered the save point
    [SerializeField] private bool isEntered;

    // Called once per frame
    private void Update() 
    {
        // Check if the player is within the save point and presses the 'Z' key
        if (isEntered && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("111");
            
            // Restore player's health and magic to maximum values
            FinalMovement.instance.health = FinalMovement.instance.maxHealth;
            FinalMovement.instance.magic = FinalMovement.instance.maxMagic;
        }
    }

    // Called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.transform.tag == "Player")
        {
            isEntered = true;
            // Debug.Log("111");
            // Debug.Log("Saved" + SceneManager.GetActiveScene());
            
            // Save the current scene name and player's position
            SaveManager.instance.scenePassword = SceneManager.GetActiveScene().name;
            SaveManager.instance.positionPassword = transform.position;
        }
    }

    // Called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit2D(Collider2D other) 
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            isEntered = false;
        }    
    }
}

