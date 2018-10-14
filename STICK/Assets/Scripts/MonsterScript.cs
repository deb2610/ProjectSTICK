using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{

    public float monsterSpeed = 0.5f; // Units per second
    public GameObject player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Seek the player
        float distanceTraveled = monsterSpeed * Time.deltaTime;
        Vector3 movement = player.transform.position - gameObject.transform.position;
        movement.z = 0;
        movement.Normalize();
        movement *= distanceTraveled;
        Debug.Log("Moving monster towards player: " + movement);
        gameObject.transform.Translate(movement);
    }
}