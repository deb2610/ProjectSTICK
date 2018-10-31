using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMonster : MonsterScript {

    public float walkSpeed = 1.0f;
    public Vector3 positionToSeek;
    public Vector3 positionToFlee;
    public float fleeSpeed = 2.5f;
    public float pausePeriod = 0.25f;
    public float lookPeriod = 2.0f;        // Pause for two seconds
    public float turnToTorchSpeed = 0.5f;   // half a second to look at the torch
    public float secondPausePeriod = 1.0f; // Process the light for a quarter second
    public float turnToFleeSpeed = .25f;    // time it takes to turn towards the woods
    public GameObject lampToLookAt;         // lämp
    public GameObject introCanvas;
    public float postAnimationDelay = 1.0f; // Seconds
    public float cameraZoomPercentage = 0.5f;
    public float thirdPausePeriod = 2.0f;    // Seconds

    State state;
    float currentStateStartFrame;
    float animationDelay = 0;

    float pauseOneStopAngle = -90;
    float angleToLookAtLanturn = -180;
    PlayerController playerControler;
    GameObject sprite;
    Camera playerCamera;
    float timeOfKill = -1.0f;           // Negative means not set
    private float cameraStartFOV;
    float cameraZ = 9.5f;

    // Represents the current state of this monster's script. For the tutorial, we'll have him
    // walk in, pause for a second while he starts burning, look at the light, start to flee, 
    // but then combust before he can leave. 
    enum State
    {
        WalkingIn,          // The monster is walking into the player's scene   (n   seconds)
        Pause,              // The monster pauses for a brief period            (.5  seconds)           
        LookAround,         // The monster pauses to look around                (2   seconds)
        NoticingTheLight,   // The monster turns toward the light               (.5  seconds)
        Pause2,             // The monster pauses to "process" the light        (.25 seconds)
        TurnToFlee,         // The monster turns to flee back to the woods      (.25 seconds)
        Fleeing,            // The monster flees into the woods                 (n   seconds)
        ResetCamera              // One final pause to transition the camera back
    }

	// Use this for initialization
	void Start () {

        // Grab the player controler
        playerControler = player.GetComponent(typeof(PlayerController)) as PlayerController;
        playerControler.TakePlayerControl();

        // This is wrapped in a try catch so that if the component is disabled, it doesn't break
        OpeningAnimationScript openingAnimation = introCanvas.GetComponent(typeof(OpeningAnimationScript)) as OpeningAnimationScript;
        if (openingAnimation.isActiveAndEnabled)
        {
            animationDelay = openingAnimation.logoFadeOutTime + 1;
        } else
        {
            animationDelay = 1;
        }
        
        sprite = gameObject.transform.Find("spookyboi-x4").gameObject;
        myHealth = monsterMaxHealth;
        monsterBurnSpeed = 3.50f;    // It should take 4 seconds to kill this monster
        playerPos = positionToSeek;
        state = State.WalkingIn;
        currentStateStartFrame = 0;
	}

    void Update()
    {
        // rip Update() isn't virtual :*(
        CalculateMovementSpeed();
        SeekPlayer();
        CheckFlashlight();
        CheckDeath();
        LookAtPlayer();
        UpdateState();
    }

    protected override void CalculateMovementSpeed()
    {
        decidedSpeed = walkSpeed;

        // Don't let the boi start walking until the intro animation has finished
        if (Time.time < animationDelay)
        {
            decidedSpeed = 0;
        }

        switch (state)
        {
            case State.WalkingIn:
                directionOfPlayer = positionToSeek - transform.position;
                break;
            case State.Pause:
            case State.LookAround:
            case State.Pause2:
            case State.NoticingTheLight:
            case State.TurnToFlee:
            case State.ResetCamera:
                directionOfPlayer = Vector3.zero;
                ghostJuice.SetActive(true);
                break;
            case State.Fleeing:
                decidedSpeed = fleeSpeed;
                directionOfPlayer = positionToFlee - transform.position;
                break;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Overridding this so the tutorial monster can't attack the player. 
        // I guess we could just let the player be ignorant and if they happen to run into
        // the static monster, punish them for taking damage, but I think it would be better 
        // if they couldn't
    }

    protected override void LookAtPlayer()
    {
        switch (state)
        {
            case State.WalkingIn:
                base.LookAtPlayer();
                break;
            case State.Pause:
                // Do Nothing
                break;
            case State.LookAround:
                float rotation = 15 * Mathf.Sin((Time.time - currentStateStartFrame) / lookPeriod * 2 * Mathf.PI) - 90;
                pauseOneStopAngle = rotation;
                sprite.transform.eulerAngles = new Vector3(0, 0, rotation);
                break;
            case State.NoticingTheLight:
                rotation = Mathf.Lerp(pauseOneStopAngle, angleToLookAtLanturn, (Time.time - currentStateStartFrame) / turnToTorchSpeed);
                sprite.transform.eulerAngles = new Vector3(0, 0, rotation);
                break;
            case State.Pause2:
                // Do nothing
                break;
            case State.TurnToFlee:
                rotation = Mathf.Lerp(angleToLookAtLanturn, -270.0f, (Time.time - currentStateStartFrame) / turnToFleeSpeed);
                sprite.transform.eulerAngles = new Vector3(0, 0, rotation);
                break;
            case State.Fleeing:
                base.LookAtPlayer();
                break;
            case State.ResetCamera:
                Vector3 finalCamPos = player.transform.position; // Ensure the camera actually makes it back to the player
                finalCamPos.z = cameraZ;
                playerCamera.orthographicSize = Mathf.Lerp(cameraZoomPercentage * cameraStartFOV, cameraStartFOV, (Time.time - currentStateStartFrame) / thirdPausePeriod);
                playerCamera.transform.position = Vector3.Lerp(positionToSeek, finalCamPos, (Time.time - currentStateStartFrame) / thirdPausePeriod);
                break;
        }
    }

    protected override void CheckFlashlight()
    {
        // Let's make this simple:
        if (state != State.WalkingIn)
        {
            TakeDamage();
        }
    }

    void UpdateState()
    {
        switch (state)
        {
            case State.WalkingIn:
                if((transform.position - positionToSeek).magnitude < 0.05)
                {
                    state = State.Pause;
                    currentStateStartFrame = Time.time;

                    Debug.Log("Setting State: Pause at " + currentStateStartFrame);
                }
                break;
            case State.Pause:
                if (Time.time - pausePeriod > currentStateStartFrame)
                {
                    Debug.Log("Pause Period: " + pausePeriod);
                    state = State.LookAround;
                    currentStateStartFrame = Time.time;

                    Debug.Log("Setting State: LookAround at " + currentStateStartFrame);
                }
                break;
            case State.LookAround:
                if (Time.time - lookPeriod > currentStateStartFrame)
                {
                    state = State.NoticingTheLight;
                    currentStateStartFrame = Time.time;

                    // Figure out what angle we need to rotate to in the next state
                    Vector3 directionOfLanturn = lampToLookAt.transform.position - transform.position;
                    float baseAngleOfLanturn = Mathf.Atan2(directionOfLanturn.y, directionOfLanturn.x) * Mathf.Rad2Deg + 90;
                    
                    // We want to rotate clockwise, therefore, we need a negative delta theta
                    while (baseAngleOfLanturn > pauseOneStopAngle)
                    {
                        baseAngleOfLanturn -= 360;
                    }
                    angleToLookAtLanturn = baseAngleOfLanturn;
                    
                    Debug.Log("Setting State: NoticingTheLight " + currentStateStartFrame);
                }
                break;
            case State.NoticingTheLight:
                if (Time.time - turnToTorchSpeed > currentStateStartFrame)
                {
                    state = State.Pause2;
                    currentStateStartFrame = Time.time;

                    Debug.Log("Setting State: Pause2 " + currentStateStartFrame);
                }
                break;
            case State.Pause2:
                if (Time.time - secondPausePeriod > currentStateStartFrame)
                {
                    state = State.TurnToFlee;
                    currentStateStartFrame = Time.time;

                    Debug.Log("Setting State: TurnToFlee " + currentStateStartFrame);
                }
                break;
            case State.TurnToFlee:
                if (Time.time - turnToFleeSpeed > currentStateStartFrame)
                {
                    state = State.Fleeing;
                    currentStateStartFrame = Time.time;
                    playerPos = positionToFlee; // Sets the monster to chase the location it came from

                    Debug.Log("Setting State: Fleeing " + currentStateStartFrame);
                }
                break;
            case State.Fleeing:
                if (timeOfKill > 0 && Time.time - postAnimationDelay > timeOfKill)
                {
                    state = State.ResetCamera;
                    currentStateStartFrame = Time.time;
                    Debug.Log("Setting State: ResetCamera " + currentStateStartFrame);
                }
                break;
            case State.ResetCamera:
                // Last state
                break;
        }
    }

    protected override void CheckDeath()
    {
        // Don't kill the monster for being out of range.
        if (myHealth <= 0)
        {
            if (timeOfKill < 0)
            {
                timeOfKill = Time.time;
                sprite.SetActive(false);
                StopEctoplasm();
            }
            if (state == State.ResetCamera && Time.time - thirdPausePeriod > currentStateStartFrame)
            {
                Vector3 finalCamPos = player.transform.position; // Ensure the camera actually makes it back to the player
                finalCamPos.z = cameraZ;
                playerCamera.transform.position = finalCamPos;
                KillMonster();
            }
        }
    }
    protected override void KillMonster()
    {
        playerControler.GivePlayerControl();
        Destroy(gameObject);
    }

    public void SetCameraStartPosition()
    {
        playerCamera = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;
        cameraZ = playerCamera.transform.position.z;
        cameraStartFOV = playerCamera.orthographicSize;
        Vector3 startCamPos = positionToSeek; // Ensure the camera actually makes it back to the player
        startCamPos.z = cameraZ;
        playerCamera.transform.position = startCamPos;
        playerCamera.orthographicSize = cameraStartFOV * cameraZoomPercentage;
    }
}
