using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Reference to the arrow prefab to be instantiated
    public GameObject ArrowPrefab;
    
    // Method to shoot the arrow
    public void Shoot()
    {
        // Instantiate the arrow prefab at the gun's position and rotation
        Instantiate(ArrowPrefab, transform.position, transform.rotation);
    }
}

