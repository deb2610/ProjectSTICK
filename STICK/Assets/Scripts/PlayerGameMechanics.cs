using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerGameMechanics : MonoBehaviour {
    public GameObject player;
    public int maxLife = 3;
    public float monsterSpawnDistance = 8;

    private int currentLife;
    private List<GameObject> monsters;

	// Use this for initialization
	void Start () {
        currentLife = maxLife;
        SceneManager scene = player.GetComponent<SceneManager>();
        monsters = scene.Monsters;
    }
	
	// Update is called once per frame
	void Update () {
        // Check for nearby monsters
        List<GameObject> nearbyMonsters = monsters.Where(m => getDistanceToObject(m) < monsterSpawnDistance).ToList();

        // Ensure nearby monsters are activated
        foreach(GameObject monster in nearbyMonsters)
        {
            monster.SetActive(true);
        }

        if (currentLife == 0)
        {
            // TODO: End the game
        }
	}

    private float getDistanceToObject(GameObject other)
    {
        return (other.transform.position - player.transform.position).magnitude;
    }
}
