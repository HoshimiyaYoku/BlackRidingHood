using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject player;
    public static bool IsOver = false;
    public Transform checkPoint;
    void Start()
    {
        gameOver.SetActive(false);
    }

    void Update()
    {
        if (GameObject.FindWithTag("CheckPoint") != null)
        {
            checkPoint = GameObject.FindWithTag("CheckPoint").transform;
            // Debug.Log(checkPoint.transform.position);
        }
        if (IsOver)
        {
            gameOver.SetActive(true);
        }
        else
        {
            gameOver.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            RestartButton();
        }
    }

    public void Setup()
    {
        IsOver = true;   
        //Debug.Log("Game Over");
    }

    public void RestartButton()
    { 
        IsOver = false;
        // Destroy(GameObject.FindWithTag("Player").gameObject);
        FinalMovement.instance.health = FinalMovement.instance.maxHealth;
        FinalMovement.instance.magic = FinalMovement.instance.maxMagic;
        FinalMovement.instance.Necklace.SetActive(true);
        FinalMovement.instance.weapon.SetActive(true);
        FinalMovement.instance.hitBox.enabled = true;
        FinalMovement.instance.rb.velocity = new Vector2(0, FinalMovement.instance.rb.velocity.y);
        FinalMovement.instance.isAlive = true;
        FinalMovement.instance.anim.SetTrigger("Rebirth");
        FinalMovement.instance.transform.position = SaveManager.instance.positionPassword;
        GameObject.Find("weapon_axe").transform.position = SaveManager.instance.positionPassword;
        Weapon.targetPos = new Vector3(FinalMovement.instance.transform.position.x, 
                                            FinalMovement.instance.transform.position.y, 0);
        FinalMovement.instance.coll.sharedMaterial = new PhysicsMaterial2D()
        {
            friction = 0.0f, bounciness = 0.0f
        };
        SceneManager.LoadScene(SaveManager.instance.scenePassword);
        
        // Debug.Log(GameObject.FindWithTag("CheckPoint").transform.position);

        //GameObject.Instantiate(player, SaveManager.Instance.lastPosition);


    }

    public void MainMenuButton()
    {
        IsOver = false;
        Destroy(GameObject.Find("Player").gameObject);
        Destroy(GameObject.Find("AudioManager").gameObject);
        Destroy(GameObject.Find("Canvas").gameObject);
        SceneManager.LoadScene("Mainmenu");
    }
}
