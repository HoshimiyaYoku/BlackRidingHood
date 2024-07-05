using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Reference to the UI Text component to display health amount
    public Text healthText;
    
    // Static variables to store current and maximum health values
    public static int healthCurrent;
    public static int healthMax;

    // Private variable to store reference to the Image component of the health bar
    private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Image component attached to this GameObject
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the fill amount of the health bar based on the current health value
        healthBar.fillAmount = (float)FinalMovement.instance.health / (float)FinalMovement.instance.maxHealth;
        
        // Update the text to display the current and maximum health values
        healthText.text = FinalMovement.instance.health.ToString() + " / " + FinalMovement.instance.maxHealth.ToString();
    }
}

