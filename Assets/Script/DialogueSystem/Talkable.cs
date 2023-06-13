using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [SerializeField] private bool isEntered;
    [SerializeField] private bool isItem;
    
    [TextArea(1, 3)]
    public string[] startLine;
    [TextArea(1, 3)]
    public string[] secondLine;
    [TextArea(1, 3)]
    public string[] thirdLine;
    [TextArea(1, 3)]
    public string[] finalLine;
    protected private Transform target;
    public GameObject dialogueBox;
    private void Start() 
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();    
    }
    private void Update() 
    {
        if(!isItem)  
            FlipTo(target);
        if (isItem)
            finalLine = thirdLine = secondLine = startLine;
        if (dialogueBox != null)
            if (DialogueManager.instance.dialogueBox.activeInHierarchy)
                dialogueBox.SetActive(false); 
            else if (!DialogueManager.instance.dialogueBox.activeInHierarchy && isEntered)
                dialogueBox.SetActive(true);
        
        if (!DialogueManager.instance.dialogueBox.activeInHierarchy && FinalMovement.instance.isGround && !FinalMovement.instance.isTalk)
        {
            if (isEntered && Input.GetKeyDown(KeyCode.Z) && GameObject.Find("Pausemenu") == null)
            {
                if (QuestManager.instance.wolfIsDie && QuestManager.instance.rabbitIsDie)
                    DialogueManager.instance.ShowDialogue(finalLine);
                else if (!QuestManager.instance.wolfIsDie && QuestManager.instance.rabbitIsDie)
                    DialogueManager.instance.ShowDialogue(thirdLine);
                else if (QuestManager.instance.wolfIsDie && !QuestManager.instance.rabbitIsDie)
                    DialogueManager.instance.ShowDialogue(secondLine);
                else if (!QuestManager.instance.wolfIsDie && !QuestManager.instance.rabbitIsDie)
                    DialogueManager.instance.ShowDialogue(startLine);
            }
        }

        if (dialogueBox != null)
        {
            dialogueBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        }    
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            FinalMovement.instance.canAttack = false;
            isEntered = true;
            if (dialogueBox != null)
                dialogueBox.SetActive(true);
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            FinalMovement.instance.canAttack = true;
            isEntered = false;
            if (dialogueBox != null)
                dialogueBox.SetActive(false);
        }    
    }

    public void FlipTo(Transform target)
    {
        if(target != null)
        {
            if(transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
