using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePropFlashlight : MonoBehaviour {
    public GameObject realFlashlight;
    public GameObject FlashlightCanvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToUpper().Contains("PLAYER"))
        {
            realFlashlight.SetActive(true);
            FlashlightCanvas.SetActive(true);
            Destroy(gameObject);
        }
    }
}
