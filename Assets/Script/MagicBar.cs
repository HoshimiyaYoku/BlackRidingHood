using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    // Reference to the UI Text component to display magic amount
    public Text magicText;
    
    // Static variables to store current and maximum magic values
    public static int magicCurrent;
    public static int magicMax;

    // Private variable to store reference to the Image component of the magic bar
    private Image magicBar;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Image component attached to this GameObject
        magicBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the fill amount of the magic bar based on the current magic value
        magicBar.fillAmount = (float)FinalMovement.instance.magic / (float)FinalMovement.instance.maxMagic;
        
        // Update the text to display the current and maximum magic values
        magicText.text = FinalMovement.instance.magic.ToString() + " / " + FinalMovement.instance.maxMagic.ToString();
    }
}

