using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public int dialogueIndex;
    List<Dialogue> dialogues;


    private void Start()
    {
        dialogues = FindObjectOfType<DialogueManager>().dialogueRepo;
    }
    void OnTriggerEnter()
    {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogues[dialogueIndex]);
    }
}
