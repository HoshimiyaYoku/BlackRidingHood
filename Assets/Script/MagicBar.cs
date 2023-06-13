using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public Text magicText;
    public static int magicCurrent;
    public static int magicMax;

    private Image magicBar;
    
    // Start is called before the first frame update
    void Start()
    {
        magicBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        magicBar.fillAmount = (float)FinalMovement.instance.magic / (float)FinalMovement.instance.maxMagic;
        magicText.text = FinalMovement.instance.magic.ToString() + " / " + FinalMovement.instance.maxMagic.ToString();
    }
}
