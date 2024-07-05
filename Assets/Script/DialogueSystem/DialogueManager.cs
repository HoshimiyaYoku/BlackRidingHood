using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Singleton instance of the DialogueManager
    public static DialogueManager instance;

    // References to the dialogue box and text UI elements
    public GameObject dialogueBox;
    public Text dialogueText, nameText;

    // Array of dialogue lines and the current line index
    [TextArea(1, 3)]
    public string[] dialogueLines;
    [SerializeField] public int currentLine;

    // Flags and variables for text scrolling effect
    private bool isScrolling;
    [SerializeField] private float textSpeed;

    // Called when the script instance is being loaded
    private void Awake() 
    {
        // Implement singleton pattern
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Display the initial dialogue line
        dialogueText.text = dialogueLines[currentLine];
    }

    // Called once per frame
    private void Update()
    {
        // Check if the dialogue box is active
        if (dialogueBox.activeInHierarchy)
        {
            // Stop player movement and set isTalk flag to true
            FinalMovement.instance.isTalk = true;
            FinalMovement.instance.rb.velocity = new Vector2(0, 0);

            // Check for player input to advance the dialogue
            if (Input.GetKeyDown(KeyCode.Z))
            {
                // Advance to the next line if text is not currently scrolling
                if (!isScrolling)
                {
                    currentLine++;
                    if (currentLine < dialogueLines.Length)
                    {
                        CheckName();
                        StartCoroutine(ScrollingText());
                    }
                    else
                        dialogueBox.SetActive(false);
                }
            }
        }
        else
        {
            // Set isTalk flag to false when dialogue box is inactive
            FinalMovement.instance.isTalk = false;
        }
    }

    // Show the dialogue with the provided lines
    public void ShowDialogue(string[] _newLines)
    {
        dialogueLines = _newLines;
        currentLine = 0;

        CheckName();

        // Start scrolling the text
        StartCoroutine(ScrollingText());
        dialogueBox.SetActive(true);
    }

    // Check if the current dialogue line contains a name
    private void CheckName()
    {
        if(dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

    // Coroutine to scroll the text one letter at a time
    private IEnumerator ScrollingText()
    {
        isScrolling = true;
        dialogueText.text = "";

        foreach (char letter in dialogueLines[currentLine].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isScrolling = false;
    }
}

