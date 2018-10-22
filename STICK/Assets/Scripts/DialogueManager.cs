using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public List<Dialogue> dialogueRepo;

    public Button theButton;
    public GameObject box;
    public Text nameText;
    public Text dialogueText;

    /*Index Refrence
     * 0 - Intro
     * 1 - Instructions
     * 2 - That flashlight wont help you for long
     * 3 - What was that sound
     * 4 - i can hear you breathing
     * 5- come out come out
     * 6 - jacket
     * 7 - smell a human
     * 8 - found your mac and cheese
     * 9 -
     * 10 - 
     * 11 - wowie
     * 12 - coming to find you
     * 13 - cheesy goodness
     * */
    

    private Queue<string> sentences; //tracks sentences
	// Use this for initialization
	void Start () {

        sentences = new Queue<string>();
        theButton.GetComponent<CanvasGroup>().alpha = 0.00f;
        box.GetComponent<CanvasGroup>().alpha = 0.00f;
    }
	
    public void StartDialogue(Dialogue dialogue)
    {
        theButton.GetComponent<CanvasGroup>().alpha = 1.00f;
        box.GetComponent<CanvasGroup>().alpha = 1.00f;
        nameText.text = dialogue.nameT;
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
        theButton.GetComponent<CanvasGroup>().alpha = 0.00f;
        box.GetComponent<CanvasGroup>().alpha = 0.00f;
        nameText.text = "";
        dialogueText.text = "";
    }
}
