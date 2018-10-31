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
        // Disable the trigger so the same dialogue doesn't get enabled twice
        Collider collider = gameObject.GetComponent(typeof(Collider)) as Collider;

        // Disable the trigger so the same dialogue doesn't get enabled twice
        collider.enabled = false;

        FindObjectOfType<DialogueManager>().StartDialogue(dialogues[dialogueIndex]);
    }
}
