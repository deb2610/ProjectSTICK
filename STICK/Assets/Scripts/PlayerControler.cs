using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    public float movementSpeed = 1f; // Units per second
    public float sprintMultiplier = 2f;
    public float joystickTolerance = 0.05f; // Lower equals more sensitive
    public GameObject camera;
    public GameObject player;

    private float cameraOffset;

	// Use this for initialization
	void Start () {
        cameraOffset = camera.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        // Keyboard Controls
        float triggerHeld = Input.GetAxis("SprintAxis");
        float sprinting = Input.GetKey(KeyCode.LeftShift)   // Left shift
            || Input.GetKey(KeyCode.RightShift)             // Right shift
            || triggerHeld > 0.1                            // Left trigger
            || triggerHeld < -0.1                           // Right trigger
            ? sprintMultiplier : 1;
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

        // Controller Support
        float xJoysticMove = Input.GetAxis("LeftJoystickX");
        float yJoysticMove = Input.GetAxis("LeftJoystickY");
        float xJoysticLook = Input.GetAxis("RightJoystickX");
        float yJoysticLook = Input.GetAxis("RightJoystickY");

        Vector3 joystickDirection = new Vector3(xJoysticMove, yJoysticMove, 0);
        if (joystickDirection.magnitude > joystickTolerance)
        {
            if(joystickDirection.magnitude > 1)
            {
                joystickDirection = joystickDirection.normalized;
            }
            joystickDirection = joystickDirection * movementDistance;
            Debug.Log("Adjusted Vector:" + joystickDirection);
            transform.Translate(joystickDirection);
        }
        joystickDirection = new Vector3(xJoysticLook, yJoysticLook, 0);
        if (joystickDirection.magnitude > joystickTolerance)
        {
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(yJoysticLook, xJoysticLook) - 90;
            player.transform.eulerAngles = new Vector3(0, 0, rotation);
        }

        // Have camera track player
        Vector3 cameraPosition = player.transform.position;
        cameraPosition.z = cameraOffset;
        camera.transform.position = cameraPosition;
    }
}
