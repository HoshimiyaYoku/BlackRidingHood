using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDoor : MonoBehaviour
{
   public string sceneName;
   public bool isIN;
    [SerializeField] private string newScenePassword;
    private void Update() 
    {
        if (!QuestManager.instance.wolfIsDie)
            gameObject.SetActive(false);
        if (isIN && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("111");
            FindObjectOfType<ChangeScene>().FadeTo(sceneName);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
            if(other.tag == "Player")
            {
                isIN = true;
                FinalMovement.instance.scenePassword = newScenePassword;
                // GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = false;
                // Debug.Log("player in");
                // SceneManager.LoadScene(sceneName);
            }   
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            isIN = false;
        }
    }
    
}
