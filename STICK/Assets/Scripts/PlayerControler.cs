using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    public float movementSpeed = 1f; // Units per second
    public float sprintMultiplier = 2f;
    public float joystickTolerance = 0.05f; // Lower equals more sensitive
    public GameObject player;
    private Transform playerModel;

    private float cameraOffset;
    private bool useMouse = true;

    /// <summary>
    /// A vector pointing in the direction of the flashlight
    /// </summary>
    public Vector3 FlashlightAngle { get; private set; }

	// Use this for initialization
	void Start () {
        playerModel = player.transform.Find("PlayerModel");
	}
	
	// Update is called once per frame
	void Update () {
        // Check for input toggle
        if(Input.GetKeyDown(KeyCode.F1))
        {
            useMouse = !useMouse;
        }

        // Keyboard Controls
        float triggerHeld = Input.GetAxis("SprintAxis");
        float sprinting = 1;
        
        // We're removing sprinting for now. It is over powered and now that the player's base speed is faster,
        // it's not as necessary
        /* Input.GetKey(KeyCode.LeftShift)   // Left shift
            || Input.GetKey(KeyCode.RightShift)             // Right shift
            || triggerHeld > 0.1                            // Left trigger
            || triggerHeld < -0.1                           // Right trigger
            ? sprintMultiplier : 1;*/
        float movementDistance = Time.deltaTime * movementSpeed * sprinting;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, movementDistance, 0));
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-movementDistance, 0, 0));
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -movementDistance, 0));
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(movementDistance, 0, 0));
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
                // If the player starts to use a joystick, assume that looking at the mouse is unnecessary
                useMouse = false;
                joystickDirection = joystickDirection.normalized;
            }
            joystickDirection = joystickDirection * movementDistance;
            Debug.Log("Adjusted Vector:" + joystickDirection);
            transform.Translate(joystickDirection);
        }
        joystickDirection = new Vector3(xJoysticLook, yJoysticLook, 0);
        if (useMouse)
        {
            LookAtMouse();
        }
        else if (joystickDirection.magnitude > joystickTolerance)
        {
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(yJoysticLook, xJoysticLook) - 90;
            playerModel.eulerAngles = new Vector3(0, 0, rotation);
        }
        
        FlashlightAngle = gameObject.transform.GetChild(0).up * -1;

        // Kill all momentum
        Rigidbody rigidbody = player.GetComponent(typeof(Rigidbody)) as Rigidbody;
        rigidbody.velocity = Vector3.zero;
    }

    private void LookAtMouse()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 flashlightDirection = mouse - screenCenter;
        float rotation = Mathf.Rad2Deg * Mathf.Atan2(flashlightDirection.y, flashlightDirection.x) - 90;
        playerModel.eulerAngles = new Vector3(0, 0, rotation);
    }
}
