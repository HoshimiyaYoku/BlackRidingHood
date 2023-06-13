using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitDialogueControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (QuestManager.instance.rabbitIsDie)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.instance.currentLine == 5)
            gameObject.SetActive(false);
    }
}
