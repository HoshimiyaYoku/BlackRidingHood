using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AXE
public class Weapon : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;//STEP 01 Weapon Rotation

    //STEP 02 Weapon Move to Mouse Position
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    private Transform playerTrans;
    public static Vector3 targetPos;//ACTUALLY This is our mousePosition
    private bool isClicked;
    private bool isRotating;

    //STEP 03
    private float throwMoment;
    private bool canComeBack;//default is false
    private bool returnWeapon;
    private bool isDamage;//MARKER avoid always damage when stop on the ground //FIXME isDamage

    //STEP 05 Effects
    public GameObject slashEffect;
    public GameObject weaponReturnEffect;

    //Trail Effect
    private TrailRenderer tr;

    public AudioSource waveAudio;


    private void Start()
    {
        playerTrans = FinalMovement.instance.transform;
        tr = GetComponentInChildren<TrailRenderer>();
        tr.enabled = false;
        
    }

    private void Update()
    { 
        
        SelfRotation();
        
        if (Input.GetButtonDown("Fire1") && isClicked == false && !FinalMovement.instance.isAttack && FinalMovement.instance.isAlive && FinalMovement.instance.canAttack)//STEP 03 !isCLicked Avoid click twice
        {
            waveAudio.Play();
            throwMoment = playerTrans.localScale.x;
 
            isClicked = true;
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);//MARKER CANNOT view the axex
            if (throwMoment == 1)
                targetPos = new Vector3(playerTrans.position.x + 8,
                                            playerTrans.position.y, 0);//MARKER SAVE the TARGET Position
            if (throwMoment == -1)
                targetPos = new Vector3(playerTrans.position.x - 8,
                                            playerTrans.position.y, 0);//MARKER SAVE the TARGET Position       
        }

        if(isClicked)
        {
            isRotating = true;//MARKER AXE Start Rotation
            transform.parent = null;
            ThrowWeapon();//MARKER If click, Throw the weapon
        }

        //IF AXE Reach the Target Position
        ReachAtMousePosition();

        //STEP 03

        // if (Input.GetMouseButtonDown(0) && canComeBack)
        // {
        //     isDamage = true;//FIXME isDamage

        //     returnWeapon = true;
        // }

        if(canComeBack)
        {
            BackWeapon();
        }

        //CORE When The AXE is back to our player 
        ReachAtPlayerPosition();

        //STEP 04 MARKER Making the weapon come with our player
        if (!isClicked && !returnWeapon && !canComeBack)
        {
            transform.position = playerTrans.position;
        }
    }

    //IF AXE Reach the Target Position
    private void ReachAtMousePosition()
    {
        if (Vector2.Distance(targetPos, transform.position) <= 0.01f)
        {
            isRotating = false;
            isDamage = false;//FIXME isDamage

            tr.enabled = false;

            canComeBack = true;
        }
    }

    //CORE When The AXE is back to our player 
    private void ReachAtPlayerPosition()
    {
        if (Vector2.Distance(transform.position, playerTrans.position) <= 0.01f)
        {
            waveAudio.Stop();
            isRotating = false;
            isDamage = false;//FIXME isDamage
            isClicked = false;

            canComeBack = false;
            returnWeapon = false;

            transform.rotation = new Quaternion(0, 0, 0, 0);//MARKER MAKING SURE the weapon is correct direction

            tr.enabled = false;
            transform.parent = playerTrans;
        }
    }

    private void SelfRotation()
    {
        if(isRotating)//STEP 02
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);//STEP 01 Weapon Rotation
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }

    //STEP 02
    private void ThrowWeapon()
    {
        isRotating = true;
        isDamage = true;//FIXME isDamage
        tr.enabled = true;

        transform.position = Vector2.MoveTowards(transform.position,  targetPos, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Enemy" && isDamage)//FIXME isDamage [Avoid The AXE stop on the ground but still takes damage to Enemy]
        {
            AttackScene.Instance.HitPause(12);
            AttackScene.Instance.CameraShake(0.1f, 0.3f);
            if(other.GetComponent<Enemy>())
            {
                if (throwMoment > 0)
                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                else if (throwMoment < 0)
                    other.GetComponent<Enemy>().GetHit(Vector2.left);

            }
            else if(other.GetComponent<NormalEnemy>())
            {
                if (throwMoment > 0)
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.right);
                else if (throwMoment < 0)
                    other.GetComponent<NormalEnemy>().GetHit(Vector2.left);
            }
            else if (other.GetComponent<Alice_AI>())
            {
                if (throwMoment > 0)
                    other.GetComponent<Alice_AI>().GetHit(Vector2.right,damage);
                else if (throwMoment < 0)
                    other.GetComponent<Alice_AI>().GetHit(Vector2.left,damage);
            }

            if (other.GetComponent<FSM>())
            {
                other.GetComponent<FSM>().isHit = true;
            }
            if (other.GetComponent<Werewolf_AI>())
            {
                other.GetComponent<Werewolf_AI>().health -= damage;
            }

            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(damage);
            else if(other.GetComponent<NormalEnemy>())
                other.GetComponent<NormalEnemy>().TakeDamage(damage);    
            Instantiate(slashEffect, transform.position, Quaternion.identity);
        }
    }

    private void BackWeapon()
    {
        isRotating = true;

        tr.enabled = true;

        transform.position = Vector2.MoveTowards(transform.position, playerTrans.position, moveSpeed * 5 * Time.deltaTime);

        //STEP 05
        if (Vector2.Distance(transform.position, playerTrans.position) <= 0.01f)
        {
            // EventSystem.instance.CameraShakeEvent(0.4f);//MARKER OB PATTERN
            Instantiate(weaponReturnEffect, playerTrans.position, Quaternion.identity);
        }
    }

}
