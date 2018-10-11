using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    public float movementSpeed = 1f; // Units per second
    public float sprintMultiplier = 2f;
    public GameObject camera;
    public GameObject player;

    private float cameraOffset;

	// Use this for initialization
	void Start () {
        cameraOffset = camera.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        float sprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? sprintMultiplier : 1;
        Debug.Log(sprinting);
        float movementDistance = Time.deltaTime * movementSpeed * sprinting;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, movementDistance, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-movementDistance, 0, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -movementDistance, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(movementDistance, 0, 0));
            player.transform.eulerAngles = new Vector3(0, 0, 270);
        }

        // Have camera track player
        Vector3 cameraPosition = player.transform.position;
        cameraPosition.z = cameraOffset;
        camera.transform.position = cameraPosition;
    }
}
