using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryScript : MonoBehaviour {
    public GameObject playerWithFlashlight;
    private int animStep;
    float yEdit;
    public float speed;

    // Use this for initialization
    void Start () {
        animStep = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Animate();
	}
    void Animate()
    {
        float yEdit = transform.position.y;
        switch (animStep)
        {
            case 0:
                yEdit += .05f * speed * Time.deltaTime;
                animStep++;
                break;
            case 1:
                yEdit += .04f * speed * Time.deltaTime;
                animStep++;
                break;
            case 2:
                yEdit += .03f * speed * Time.deltaTime;
                animStep++;
                break;
            case 3:
                yEdit += .02f * speed * Time.deltaTime;
                animStep++;
                break;
            case 4:
                yEdit += .01f * speed * Time.deltaTime;
                animStep++;
                break;
            case 5:
                yEdit -= .01f * speed * Time.deltaTime;
                animStep++;
                break;
            case 6:
                yEdit -= .02f * speed * Time.deltaTime;
                animStep++;
                break;
            case 7:
                yEdit -= .03f * speed * Time.deltaTime;
                animStep++;
                break;
            case 8:
                yEdit -= .04f * speed * Time.deltaTime;
                animStep++;
                break;
            case 9:
                yEdit -= .05f * speed * Time.deltaTime;
                animStep= 0;
                break;
            default:
                break;
        }
        gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, yEdit, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        playerWithFlashlight.GetComponent<FlashlightManager>().RestoreHealth(1);
        gameObject.SetActive(false);

    }
}
