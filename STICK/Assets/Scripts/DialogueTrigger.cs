using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public int dialogueIndex;
    List<Dialogue> dialogues = FindObjectOfType<DialogueManager>().dialogueRepo;

    void OnTriggerEnter()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogues[dialogueIndex]);
    }
}
