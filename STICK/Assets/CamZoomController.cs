using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoomController : MonoBehaviour {
    Camera playerCamera;
    public float Zoom1 = 4;
    public float Zoom2 = 6;

    public float ypos1;
    public float ypos2;

    public float duration = 1.0f;
    private float elapsed = 0.0f;
    private bool transition = false;

    public bool smallCam;
    // Use this for initialization
    void Start () {
        playerCamera = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;
        smallCam = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (transition)
        {
            if (smallCam)
            {
                CamZoomToggle(Zoom1,Zoom2);
            }
            else
            {
              CamZoomToggle(Zoom2,Zoom1);
            }
        }
    }
    void CamZoomToggle(float zoomFrom, float zoomTo)
    {
        elapsed += Time.deltaTime / duration;
        Camera.main.orthographicSize = Mathf.Lerp(zoomFrom, zoomTo, elapsed);
        Debug.Log("Make Big");       
        if (elapsed > 1.0f)
        {
            transition = false;
            if (smallCam == true)
            {
                smallCam = false;
            }
            else
            {
                smallCam = true;
            }
        }


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToUpper().Contains("PLAYER"))
        {
            transition = true;
            elapsed = 0.0f;
            
        }
    }
}
