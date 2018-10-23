using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameCanvas : MonoBehaviour {

    public Image win;
    public Image lose;
    public Image background;
    public bool gameOverMan;
    public float cardFadeTime;
    protected bool didIWin;

    // Use this for initialization
    void Start () {
        gameOverMan = false;	
	}
	
	// Update is called once per frame
	void Update () {
        if (gameOverMan)
        {

            background.GetComponent<CanvasGroup>().alpha += .01f;
            
            if (didIWin)
            {
                win.GetComponent<CanvasGroup>().alpha += .01f;
            }
            else
            {
                lose.GetComponent<CanvasGroup>().alpha += .01f;
            }
        }
	}
    public void DisplayEndCanvas(bool _didIWin)
    {
        gameOverMan = true;
        didIWin = _didIWin;
    }
}
