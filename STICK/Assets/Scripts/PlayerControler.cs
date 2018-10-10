using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    public float movementSpeed = 1f; // Units per second
    public float sprintMultiplier = 2f;
    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float sprinting = Input.GetKey(KeyCode.LeftShift | KeyCode.RightShift) ? sprintMultiplier : 1;
        float movementDistance = Time.deltaTime * movementSpeed * sprinting;
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, movementDistance, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-movementDistance, 0, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -movementDistance, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(movementDistance, 0, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 270);
        }

    }
}
