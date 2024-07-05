using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDoor : MonoBehaviour
{
    // Name of the scene to load
    public string sceneName;

    // Flag to check if the player is in the trigger zone
    public bool isIN;

    // Password for the new scene
    [SerializeField] private string newScenePassword;

    // Called once per frame
    private void Update() 
    {
        // Disable the warp door if the quest condition is not met
        if (!QuestManager.instance.wolfIsDie)
            gameObject.SetActive(false);

        // Check if the player is in the trigger zone and presses the 'Z' key
        if (isIN && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("111");
            FindObjectOfType<ChangeScene>().FadeTo(sceneName);
        }
    }

    // Called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Check if the collider belongs to the player
        if(other.tag == "Player")
        {
            isIN = true;

            // Set the player's scene password to the new password
            FinalMovement.instance.scenePassword = newScenePassword;

            // Uncomment the following lines to debug or directly load the scene
            // GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = false;
            // Debug.Log("player in");
            // SceneManager.LoadScene(sceneName);
        }   
    }

    // Called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit2D(Collider2D other) 
    {
        // Check if the collider belongs to the player
        if(other.tag == "Player")
        {
            isIN = false;
        }
    }
}

