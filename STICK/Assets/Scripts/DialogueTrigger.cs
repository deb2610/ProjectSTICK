using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;

    void OnTriggerEnter()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
