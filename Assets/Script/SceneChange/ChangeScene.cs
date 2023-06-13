using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public Image blackImage;
    [SerializeField] private float alpha;

    private void Awake() {
        if (FinalMovement.instance != null)
        {
            FinalMovement.instance.anim.SetFloat("running", 0);
            FinalMovement.instance.anim.SetBool("Jump", false);
            FinalMovement.instance.anim.SetBool("Fall", false);
        }
    }
    private void Start() 
    {
        GameObject.Find("Player").gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        
        StartCoroutine(FadeIn());
            
    }

    public void FadeTo(string _sceneName)
    {
        StartCoroutine(Fadeout(_sceneName));
    }    
    IEnumerator FadeIn()
    {
        
        alpha = 1;

        while(alpha > 0)
        {
            alpha -= Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }

        GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = true;
    }

    IEnumerator Fadeout(string sceneName)
    {
        GameObject.Find("Player").gameObject.GetComponent<FinalMovement>().enabled = false;
        GameObject.Find("Player").gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        
        alpha = 0;

        while(alpha < 1)
        {
            alpha += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
