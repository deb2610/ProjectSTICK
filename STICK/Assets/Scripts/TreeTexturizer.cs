using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeTexturizer : MonoBehaviour {
    public Sprite pineTop;
    public Sprite pineMid;
    public Sprite pineBot;
    public Sprite mapleTop;
    public Sprite mapleMid;
    public Sprite mapleBot;
    public int treeType;
    public GameObject treeTopObject;
    public GameObject treeMidObject;
    public GameObject treeBotObject;


    // Use this for initialization
    void Start () {
        treeType = Random.Range(0, 2);
        switch (treeType)
        {
            case 0:
                treeTopObject.GetComponent<SpriteRenderer>().sprite = pineTop;
                treeMidObject.GetComponent<SpriteRenderer>().sprite = pineMid;
                treeBotObject.GetComponent<SpriteRenderer>().sprite = pineBot;
                break;
            case 1:
                treeTopObject.GetComponent<SpriteRenderer>().sprite = mapleTop;
                treeMidObject.GetComponent<SpriteRenderer>().sprite = mapleMid;
                treeBotObject.GetComponent<SpriteRenderer>().sprite = mapleBot;
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
