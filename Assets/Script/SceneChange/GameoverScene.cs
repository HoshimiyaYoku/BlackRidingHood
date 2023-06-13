using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverScene : MonoBehaviour
{
    public string sceneName;
    [SerializeField] private string newScenePassword;
    private void OnTriggerEnter2D(Collider2D other) 
    {
            if(other.tag == "Player")
            {
                Destroy(GameObject.Find("Player").gameObject);
                Destroy(GameObject.Find("AudioManager").gameObject);
                Destroy(GameObject.Find("Canvas").gameObject);
                FindObjectOfType<ChangeScene>().FadeTo(sceneName);
            }   
    }
}
