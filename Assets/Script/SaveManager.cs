using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    // Start is called before the first frame update
    public string scenePassword;
    public Vector3 positionPassword;
    private void Awake() 
    {
        
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
    }

    void Update() 
    {
        // Debug.Log(scenePassword);    
    }
}
