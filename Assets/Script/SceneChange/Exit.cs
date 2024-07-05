using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    // Name of the scene to load
    public string sceneName;

    // Password for the new scene
    [SerializeField] private string newScenePassword;

    // Called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Check if the collider belongs to the player
        if(other.tag == "Player")
        {
            // Set the player's scene password to the new password
            FinalMovement.instance.scenePassword = newScenePassword;

            // Uncomment the following lines to debug or directly load the scene
            // GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = false;
            // Debug.Log("player in");
            // SceneManager.LoadScene(sceneName);

            // Use the ChangeScene script to fade out and load the new scene
            FindObjectOfType<ChangeScene>().FadeTo(sceneName);
        }   
    }
}

