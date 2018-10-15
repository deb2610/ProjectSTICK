using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public float monsterSpeed = 0.5f; // Units per second
    public GameObject player;

    public float monsterKillDistance = 2.5f;
    public float monsterKillAngle = 15.0f; //degrees

    private Vector3 directionOfPlayer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SeekPlayer();
        CheckKill();
    }

    void CalculateMovementSpeed()
    {

    }

    void SeekPlayer()
    {
        // Seek the player
        float distanceTraveled = monsterSpeed * Time.deltaTime;
        Vector3 movement = player.transform.position - gameObject.transform.position;
        directionOfPlayer = movement;
        movement.z = 0;
        movement.Normalize();
        movement *= distanceTraveled;
        Debug.Log("Moving monster towards player: " + movement);
        gameObject.transform.Translate(movement);
    }

    void CheckKill()
    {
        if (directionOfPlayer.magnitude <= monsterKillDistance)
        {
            PlayerControler playerController = player.GetComponent(typeof(PlayerControler)) as PlayerControler;

            float myAngle = Vector3.Angle(playerController.FlashlightAngle, directionOfPlayer);
            float angleOffset = Mathf.Abs(myAngle);
            if (angleOffset <= monsterKillAngle)
            {
                Debug.Log("Distance: " + directionOfPlayer.magnitude);
                Debug.Log("Angle Offset: " + angleOffset);
                Debug.Log("Angle to player : " + myAngle);
                KillMonster();
            }
        }
    }

    /// <summary>
    /// This method removes the game object. It's a separate method in case we want to add animation later
    /// </summary>
    void KillMonster()
    {
        // Let the player object know that there is one fewer monsters
        PlayerGameMechanics playerGameMechanics = player.GetComponent(typeof(PlayerGameMechanics)) as PlayerGameMechanics;
        playerGameMechanics.RemoveMonster(gameObject);

        // Remove this game object
        Destroy(gameObject);
    }
}