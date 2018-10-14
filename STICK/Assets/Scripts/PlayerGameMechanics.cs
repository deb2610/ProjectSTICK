using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerGameMechanics : MonoBehaviour {
    public GameObject player;
    public int maxLife = 3;
    public float monsterSpawnDistance = 5;
    public GameObject monsterPrefab;
    public List<GameObject> Monsters { get; private set; }
    public float monsterSpawnRate = 2; //% chance
    private float timeOfLastMonster = 0;
    public int maxMonsters = 5;
    public float monsterSpeed = 0.1f;

    private int currentLife;
    private List<GameObject> monsters;

	// Use this for initialization
	void Start () {
        currentLife = maxLife;
        Monsters = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {

        // Maybe make a spookie boi
        if (
                Monsters.Count < maxMonsters && 
                (Random.value * 100 < monsterSpawnRate || Time.time - timeOfLastMonster > 15)
            )
        {
            timeOfLastMonster = Time.time;
            SpawnMonster();
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

    private void SpawnMonster()
    {
        // Pick a random direction
        Vector3 monsterSpawn = new Vector3(monsterSpawnDistance, 0, 0);
        monsterSpawn = Quaternion.AngleAxis(Random.value * 360, Vector3.forward) * monsterSpawn;

        Debug.Log(transform.position);

        // Instantiate the monster
        GameObject newMonster = Instantiate(monsterPrefab, transform.position + monsterSpawn, Quaternion.identity);
        MonsterScript monsterController = newMonster.AddComponent(typeof(MonsterScript)) as MonsterScript;
        monsterController.player = gameObject;
        monsterController.monsterSpeed = monsterSpeed;
        Monsters.Add(newMonster);
    }
}
