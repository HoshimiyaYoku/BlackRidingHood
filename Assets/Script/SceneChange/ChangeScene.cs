using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Reference to the UI Image for fade effect
    public Image blackImage;
    
    // Alpha value for fade effect
    [SerializeField] private float alpha;

    // Called when the script instance is being loaded
    private void Awake() 
    {
        // Reset player animation states if the player exists
        if (FinalMovement.instance != null)
        {
            FinalMovement.instance.anim.SetFloat("running", 0);
            FinalMovement.instance.anim.SetBool("Jump", false);
            FinalMovement.instance.anim.SetBool("Fall", false);
        }
    }

    // Start is called before the first frame update
    private void Start() 
    {
        // Stop player movement
        GameObject.Find("Player").gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        
        // Start fade-in effect
        StartCoroutine(FadeIn());
    }

    // Method to start fade-out effect and change scene
    public void FadeTo(string _sceneName)
    {
        StartCoroutine(Fadeout(_sceneName));
    }    

    // Coroutine to handle fade-in effect
    IEnumerator FadeIn()
    {
        alpha = 1;

        // Gradually decrease alpha to create fade-in effect
        while(alpha > 0)
        {
            alpha -= Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }

        // Enable player movement after fade-in
        GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = true;
    }

    // Coroutine to handle fade-out effect
    IEnumerator Fadeout(string sceneName)
    {
        // Disable player movement
        GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = false;
        GameObject.Find("Player").gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        
        alpha = 0;

        // Gradually increase alpha to create fade-out effect
        while(alpha < 1)
        {
            alpha += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}
