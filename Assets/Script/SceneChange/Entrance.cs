using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{   
    public string entrancePassword;
    private void Start() 
    {     
        if(FinalMovement.instance.scenePassword == entrancePassword)
        {
            FinalMovement.instance.transform.position = transform.position;
            GameObject.Find("weapon_axe").transform.position = transform.position;
            Weapon.targetPos = new Vector3(FinalMovement.instance.transform.position.x, 
                                                FinalMovement.instance.transform.position.y, 0);
        }
        else
        {
            // Debug.Log("Wrong PW"); 
        }
    }
}
