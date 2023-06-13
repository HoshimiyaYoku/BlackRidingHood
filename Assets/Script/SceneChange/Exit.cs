using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public string sceneName;
    [SerializeField] private string newScenePassword;
    private void OnTriggerEnter2D(Collider2D other) 
    {
            if(other.tag == "Player")
            {
                FinalMovement.instance.scenePassword = newScenePassword;
                // GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = false;
                // Debug.Log("player in");
                // SceneManager.LoadScene(sceneName);
                FindObjectOfType<ChangeScene>().FadeTo(sceneName);
            }   
    }
}
