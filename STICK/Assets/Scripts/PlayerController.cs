using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movementSpeed = 1f; // Units per second
    public float sprintMultiplier = 2f;
    public float joystickTolerance = 0.05f; // Lower equals more sensitive
    public GameObject player;
    private Transform playerModel;

    private float cameraOffset;
    private bool useMouse = true;
    private bool playerHasControl = true;
    public GameObject canvasTextBox;
    public GameObject canvasButton;
    public GameObject light1;
    public GameObject light2;
    public GameObject light3;
    public GameObject light4;
    public GameObject bossHead;
    public GameObject bossArm1;
    public GameObject bossArm2;
    public GameObject bossMane;
    public GameObject bossTail;
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

        PausedInput();

        if (playerHasControl)
        {
            ProcessInput();
        }
    }

    void PausedInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerGameMechanics playerGameMechanics = player.GetComponent(typeof(PlayerGameMechanics)) as PlayerGameMechanics;
            playerGameMechanics.TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<DialogueManager>().theButton.onClick.Invoke();
            //GetComponent<SceneManager>().GetComponent<DialogueManager>().DisplayNextSentence();
        }
    }

    void ProcessInput()
    {
        // Check for input toggle
        if (Input.GetKeyDown(KeyCode.F1))
        {
            useMouse = !useMouse;
        }

        // Keyboard Controls
        //float triggerHeld = Input.GetAxis("SprintAxis");
        float sprinting = 1;

        // We're removing sprinting for now. It is over powered and now that the player's base speed is faster,
        // it's not as necessary
        /* Input.GetKey(KeyCode.LeftShift)   // Left shift
            || Input.GetKey(KeyCode.RightShift)             // Right shift
            || triggerHeld > 0.1                            // Left trigger
            || triggerHeld < -0.1                           // Right trigger
            ? sprintMultiplier : 1;*/
        float movementDistance = Time.deltaTime * movementSpeed * sprinting;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<DialogueManager>().theButton.onClick.Invoke();
            //GetComponent<SceneManager>().GetComponent<DialogueManager>().DisplayNextSentence();
        }
        //gdc movie stuff
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 3.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = 1.5f;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowTextbox();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShowLights();
        }
        if (Input.GetKey(KeyCode.B))
        {
            FadeBossOut();
        }
        if (Input.GetKey(KeyCode.N))
        {
            FadeBossIn();
        }


        // Controller Support
        float xJoysticMove = Input.GetAxis("LeftJoystickX");
        float yJoysticMove = Input.GetAxis("LeftJoystickY");
        float xJoysticLook = Input.GetAxis("RightJoystickX");
        float yJoysticLook = Input.GetAxis("RightJoystickY");

        Vector3 joystickDirection = new Vector3(xJoysticMove, yJoysticMove, 0);
        if (joystickDirection.magnitude > joystickTolerance)
        {
            if (joystickDirection.magnitude > 1)
            {
                // If the player starts to use a joystick, assume that looking at the mouse is unnecessary
                useMouse = false;
                joystickDirection = joystickDirection.normalized;
            }
            joystickDirection = joystickDirection * movementDistance;
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

    public void GivePlayerControl()
    {
        playerHasControl = true;
    }

    public void TakePlayerControl()
    {
        playerHasControl = false;
    }
    public void ShowTextbox()
    {
        if (canvasButton.activeSelf == false)
        {
            canvasTextBox.GetComponent<CanvasGroup>().alpha = 1;
            canvasButton.GetComponent<CanvasGroup>().alpha = 1;
            canvasTextBox.SetActive(true);
            canvasButton.SetActive(true);
        }
        else
        {
            canvasTextBox.GetComponent<CanvasGroup>().alpha = 0;
            canvasButton.GetComponent<CanvasGroup>().alpha = 0;
            canvasTextBox.SetActive(false);
            canvasButton.SetActive(false);
        }
    }
    public void ShowLights()
    {
        if (light1.activeSelf == false)
        {
            light1.SetActive(true);
            light2.SetActive(true);
            light3.SetActive(true);
            light4.SetActive(true);
        }
        else
        {
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
            light4.SetActive(false);
        }
    }
    public void FadeBossOut()
    {
        var color = bossHead.GetComponent<Renderer>().material.color;
        var alpha = color.a;
        alpha -= .01f;
        if (alpha <= 0.0f)
        {
            alpha = 0.0f;
        }
        //Debug.Log(alpha);
        Color newcolor = new Color(color.r, color.g, color.b, alpha);
        //Debug.Log(newcolor.a);
        bossHead.GetComponent<Renderer>().material.color = newcolor;
        bossArm1.GetComponent<Renderer>().material.color = newcolor;
        bossArm2.GetComponent<Renderer>().material.color = newcolor;
        bossMane.GetComponent<Renderer>().material.color = newcolor;
        bossTail.GetComponent<Renderer>().material.color = newcolor;
    }
    public void FadeBossIn()
    {
        var color = bossHead.GetComponent<Renderer>().material.color;
        var alpha = color.a;
        alpha += .01f;
        if (alpha >= 1.0f)
        {
            alpha = 1.0f;
        }
        Color newcolor = new Color(color.r, color.g, color.b, alpha);
        //Debug.Log(newcolor.a);
        bossHead.GetComponent<Renderer>().material.color = newcolor;
        bossArm1.GetComponent<Renderer>().material.color = newcolor;
        bossArm2.GetComponent<Renderer>().material.color = newcolor;
        bossMane.GetComponent<Renderer>().material.color = newcolor;
        bossTail.GetComponent<Renderer>().material.color = newcolor;
    }
}
