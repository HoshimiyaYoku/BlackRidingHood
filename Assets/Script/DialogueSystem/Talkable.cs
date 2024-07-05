using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    // Flag to check if the player has entered the talkable area
    [SerializeField] private bool isEntered;

    // Flag to check if the object is an item
    [SerializeField] private bool isItem;

    // Dialogue lines for different states
    [TextArea(1, 3)]
    public string[] startLine;
    [TextArea(1, 3)]
    public string[] secondLine;
    [TextArea(1, 3)]
    public string[] thirdLine;
    [TextArea(1, 3)]
    public string[] finalLine;

    // Reference to the player's transform
    protected private Transform target;

    // Reference to the dialogue box GameObject
    public GameObject dialogueBox;

    // Start is called before the first frame update
    private void Start() 
    {
        // Get the player's transform
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();    
    }

    // Update is called once per frame
    private void Update() 
    {
        // Flip the character to face the player if not an item
        if(!isItem)  
            FlipTo(target);

        // Set all dialogue lines to startLine if it's an item
        if (isItem)
            finalLine = thirdLine = secondLine = startLine;

        // Handle the dialogue box visibility
        if (dialogueBox != null)
            if (DialogueManager.instance.dialogueBox.activeInHierarchy)
                dialogueBox.SetActive(false); 
            else if (!DialogueManager.instance.dialogueBox.activeInHierarchy && isEntered)
                dialogueBox.SetActive(true);

        // Handle dialogue interaction
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

        // Adjust dialogue box orientation
        if (dialogueBox != null)
        {
            dialogueBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        }    
    }

    // Called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            FinalMovement.instance.canAttack = false;
            isEntered = true;
            if (dialogueBox != null)
                dialogueBox.SetActive(true);
        }    
    }

    // Called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit2D(Collider2D other) 
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            FinalMovement.instance.canAttack = true;
            isEntered = false;
            if (dialogueBox != null)
                dialogueBox.SetActive(false);
        }    
    }

    // Flip the character to face the target
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

