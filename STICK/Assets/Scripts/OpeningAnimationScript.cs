using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningAnimationScript : MonoBehaviour {
    //variables
    public Image logo;
    public Image background;
    public float logoFadeOutTime;
    private Color colorToFadeTo;
    private bool playIntro;

    // Use this for initialization
    void Start () {
        playIntro = true;

    }

    // Update is called once per frame
    void Update () {
        if (playIntro)
        {
            if (logoFadeOutTime >= Time.time)
            {
                logo.GetComponent<CanvasGroup>().alpha += .01f;
            }
            else
            {
                logo.GetComponent<CanvasGroup>().alpha -= .005f;
                background.GetComponent<CanvasGroup>().alpha -= .01f;
                if (logo.GetComponent<CanvasGroup>().alpha == 0.0f)
                {
                    playIntro = false;
                }

            }
        }
        
        //if (logo.color.a == 1)
        //{
        //    logo.CrossFadeAlpha(0.0f, 20.0f, true);
        //    background.CrossFadeAlpha(0.0f, 4.0f, true);
        //}
    }
}
