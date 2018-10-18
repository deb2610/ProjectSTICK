using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public List<Dialogue> dialogueRepo;

    public Text nameText;
    public Text dialogueText;

    /*Index Refrence
     * 0 - Intro
     * 1 - Instructions
     * 
     * */
    

    private Queue<string> sentences; //tracks sentences
	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}
	
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueText.name = dialogue.name;
        sentences.Clear();

        foreach(string s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }
        DisplayNextSentence();
 
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        else
        {
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    public void EndDialogue()
    {
        nameText.text = "";
        dialogueText.text = "";
    }
}
