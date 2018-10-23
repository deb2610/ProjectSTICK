using System;
using System.Collections;
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
    protected float decidedSpeed;

    public float monsterMaxHealth = 100.0f; // Percent
    public float monsterBurnSpeed = 0.5f;   // The number of seconds it takes to kill a monster
    protected float myHealth;

    public float monsterKillDistance = 2.5f;
    public float monsterKillAngle = 22.5f; //degrees

    protected Vector3 directionOfPlayer;
    protected Vector3 playerPos;

    private float angleBetween;
    public float angelFix;
    public GameObject ghostJuice;

    // Use this for initialization
    void Start()
    {
        myHealth = monsterMaxHealth;
        ghostJuice = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //transorm correction
        //gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y, 9.75f);
        //transform.LookAt(player.transform);

        CalculateMovementSpeed();
        SeekPlayer();
        CheckFlashlight();
        CheckDeath();
        LookAtPlayer();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        AttackPlayer();
    }

    protected virtual void CalculateMovementSpeed()
    {
        SceneManager sceneManager = player.GetComponent(typeof(SceneManager)) as SceneManager;
        directionOfPlayer = player.transform.position - gameObject.transform.position;
        playerPos = player.transform.position;
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

    protected virtual void SeekPlayer()
    {
        // Seek the player
        float distanceTraveled = decidedSpeed * Time.deltaTime;
        Vector3 movement = directionOfPlayer;
        movement.z = 0;
        movement.Normalize();
        movement *= distanceTraveled;
        gameObject.transform.Translate(movement);
    }

    protected virtual void CheckFlashlight()
    {
        if (directionOfPlayer.magnitude <= monsterKillDistance)
        {
            PlayerControler playerController = player.GetComponent(typeof(PlayerControler)) as PlayerControler;
            
            float myAngle = Vector3.Angle(playerController.FlashlightAngle, directionOfPlayer);
            float angleOffset = Mathf.Abs(myAngle);
            if (angleOffset <= monsterKillAngle)
            {
                // TODO: Put Particle Emiter here
                SpewEctoplasm();
                TakeDamage();
            }
            else
            {
                StopEctoplasm();
            }
        }
    }

    protected void StopEctoplasm()
    {
        ghostJuice.SetActive(false);
    }

    private void SpewEctoplasm()
    {
        ghostJuice.SetActive(true);
    }

    protected void TakeDamage()
    {
        float damageDone = Time.deltaTime / monsterBurnSpeed * monsterMaxHealth;
        myHealth -= damageDone;
    }

    protected virtual void CheckDeath()
    {
        if (directionOfPlayer.magnitude > monsterRange)
        {
            KillMonster();
        }
        if (myHealth <= 0)
        {
            KillMonster();
        }
    }

    protected virtual void LookAtPlayer()
    {
        //Debug.DrawRay(transform.position, distanceVector, Color.green);
        GameObject sprite = gameObject.transform.Find("spookyboi-x4").gameObject;
        if (transform.position.y > playerPos.y)
        {
            float angleBetween = Vector3.Angle(Vector3.right, -directionOfPlayer);
            sprite.transform.eulerAngles = new Vector3(0, 0, angleBetween - 90);
        }
        else
        {
            float angleBetween = Vector3.Angle(Vector3.right, directionOfPlayer);
            sprite.transform.eulerAngles = new Vector3(0, 0, angleBetween + 90);
        }
        //Debug.DrawRay(transform.position, transform.forward,Color.red);
    }

    /// <summary>
    /// This method removes the game object. It's a separate method in case we want to add animation later
    /// </summary>
    protected virtual void KillMonster()
    {
        // Let the player object know that there is one fewer monsters
        PlayerGameMechanics playerGameMechanics = player.GetComponent(typeof(PlayerGameMechanics)) as PlayerGameMechanics;
        playerGameMechanics.KillMonster(gameObject);

        //stops particle system
        StopEctoplasm();
        // Remove this game object
        Destroy(gameObject);
    }

    protected virtual void AttackPlayer() 
    {
        PlayerGameMechanics playerGameMechanics = player.GetComponent<PlayerGameMechanics>();
        playerGameMechanics.AttackedByMonster(gameObject);

        // Remove this game object
        Destroy(gameObject);
    }
}