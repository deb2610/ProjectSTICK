﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public float monsterSpeed = 1.2f; // Units per second speed in open air
    public float monsterChaseSpeed = 3.0f; // Units per second speed to LERP to while chasing
    public float monsterInWallSpeed = 1.0f; // Units per second speed while in walls
    public float monsterChaseRange = 3.0f; // Units distance to start chasing from
    public float monsterRange = 8.0f;   // If the monster gets this far away from the player, it will despawn
    public GameObject player;
    private float decidedSpeed;

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
        CalculateMovementSpeed();
        SeekPlayer();
        CheckKill();
    }
    private void OnTriggerEnter(Collider other)
    {
        AttackPlayer();
    }

    void CalculateMovementSpeed()
    {
        SceneManager sceneManager = player.GetComponent(typeof(SceneManager)) as SceneManager;
        directionOfPlayer = player.transform.position - gameObject.transform.position;
        if(sceneManager.IsPointInMazeBlock(transform.position))
        {
            decidedSpeed = monsterInWallSpeed;
        }
        else if(directionOfPlayer.magnitude <= monsterChaseRange)
        {
            decidedSpeed = Mathf.Lerp(monsterSpeed, monsterChaseSpeed, monsterChaseRange - directionOfPlayer.magnitude);
        }
        else
        {
            decidedSpeed = monsterSpeed;
        }
    }

    void SeekPlayer()
    {
        // Seek the player
        float distanceTraveled = decidedSpeed * Time.deltaTime;
        Vector3 movement = directionOfPlayer;
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
                Debug.Log("Monster killed by Flashlight");
                KillMonster();
            }
        }
        if (directionOfPlayer.magnitude > monsterRange)
        {
            Debug.Log("Monster killed by range");
            KillMonster();
        }
    }

    /// <summary>
    /// This method removes the game object. It's a separate method in case we want to add animation later
    /// </summary>
    void KillMonster()
    {
        // Let the player object know that there is one fewer monsters
        PlayerGameMechanics playerGameMechanics = player.GetComponent(typeof(PlayerGameMechanics)) as PlayerGameMechanics;
        playerGameMechanics.KillMonster(gameObject);

        // Remove this game object
        Destroy(gameObject);
    }

    void AttackPlayer() 
    {
        PlayerGameMechanics playerGameMechanics = player.GetComponent(typeof(PlayerGameMechanics)) as PlayerGameMechanics;
        playerGameMechanics.AttackedByMonster(gameObject);

        // Remove this game object
        Destroy(gameObject);
    }
}