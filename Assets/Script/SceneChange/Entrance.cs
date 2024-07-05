using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{   
    // Password for the entrance to check against the player's scene password
    public string entrancePassword;

    // Start is called before the first frame update
    private void Start() 
    {     
        // Check if the player's scene password matches the entrance password
        if(FinalMovement.instance.scenePassword == entrancePassword)
        {
            // Move the player and weapon to the entrance position
            FinalMovement.instance.transform.position = transform.position;
            GameObject.Find("weapon_axe").transform.position = transform.position;
            Weapon.targetPos = new Vector3(FinalMovement.instance.transform.position.x, 
                                           FinalMovement.instance.transform.position.y, 0);
        }
        else
        {
            // Uncomment the following line to debug if the password is incorrect
            // Debug.Log("Wrong PW"); 
        }
    }
}

