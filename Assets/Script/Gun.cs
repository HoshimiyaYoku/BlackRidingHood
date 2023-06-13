using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject ArrowPrefab;
    
    public void Shoot()
    {
        Instantiate(ArrowPrefab, transform.position, transform.rotation);
    }
}
