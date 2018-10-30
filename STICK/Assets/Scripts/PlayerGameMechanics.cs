using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerGameMechanics : MonoBehaviour
{
    public GameObject player;
    public int maxLife = 3;
    public float monsterSpawnDistance = 5;
    public GameObject monsterPrefab;
    public List<GameObject> Monsters { get; private set; }

    // Variables to pass along to the monsters
    public float monsterSpeed = 0.1f;
    public float monsterChaseSpeed = 3.0f; // Units per second speed to LERP to while chasing
    public float monsterInWallSpeed = 1.0f; // Units per second speed while in walls
    public float monsterChaseRange = 3.0f; // Units distance to start chasing from
    public float monsterRange = 8.0f;   // If the monster gets this far away from the player, it will despawn
    public float monsterMaxHealth = 100.0f; // Percent
    public float monsterBurnSpeed = 0.5f;   // The number of seconds it takes to kill a monster
    public float monsterKillDistance = 2.5f;
    public float monsterKillAngle = 22.5f; //degrees

    public float monsterSpawnRate = 2; //% chance
    private float timeOfLastMonster = 0;
    public int maxMonsters = 5;

    public int currentLife;
    private List<GameObject> monsters;
    private bool shouldMonstersSpawn;

    public Canvas EndCanvas;

    private bool isPaused = false;

    // Use this for initialization
    void Start()
    {
        currentLife = maxLife;
        Monsters = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,Vector3.right,Color.yellow);
        
        // Maybe make a spookie boi

        if (
                shouldMonstersSpawn &&  // Don't start spawning monsters until the player hits the trigger
                Monsters.Count < maxMonsters && // Don't spawn more than MaxMonsters
                (Random.value * 100 < monsterSpawnRate || Time.time - timeOfLastMonster > 15) 
            )
        {
            timeOfLastMonster = Time.time;
            SpawnMonster();
        }

        if (currentLife == 0)
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
            EndCanvas.GetComponent<EndGameCanvas>().DisplayEndCanvas(false);
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

        // Instantiate the monster
        GameObject newMonster = Instantiate(monsterPrefab, transform.position + monsterSpawn, Quaternion.identity);
        MonsterScript monsterController = newMonster.AddComponent(typeof(MonsterScript)) as MonsterScript;

        // Forward all of the settings from the Unity editor
        monsterController.player = gameObject;
        monsterController.monsterSpeed = monsterSpeed;
        monsterController.monsterChaseSpeed = monsterChaseSpeed;
        monsterController.monsterInWallSpeed = monsterInWallSpeed;
        monsterController.monsterChaseRange = monsterChaseRange;
        monsterController.monsterRange = monsterRange;
        monsterController.monsterMaxHealth = monsterMaxHealth;
        monsterController.monsterBurnSpeed = monsterBurnSpeed;
        monsterController.monsterKillDistance = monsterKillDistance;
        monsterController.monsterKillAngle = monsterKillAngle;

        Monsters.Add(newMonster);
    }

    /// <summary>
    /// Use this method for a successful kill on a monster with the flashlight
    /// </summary>
    /// <param name="monster">The monster that was killed</param>
    public void KillMonster(GameObject monster)
    {
        Monsters.Remove(monster);
    }

    /// <summary>
    /// Use this method for a successful attack from a monster
    /// </summary>
    /// <param name="monster">The monster that attacked</param>
    public void AttackedByMonster(GameObject monster)
    {
        Monsters.Remove(monster);
        currentLife--;

        FlashlightManager flashlightManager = gameObject.GetComponent(typeof(FlashlightManager)) as FlashlightManager;
        flashlightManager.ReduceIntensity();
    }

    public void StartMonsterSpawn()
    {
        shouldMonstersSpawn = true;
    }

    public void StopMonsterSpawn()
    {
        shouldMonstersSpawn = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    public void PauseGame()
    {
        // Revoke player control
        PlayerController controller = GetComponent(typeof(PlayerController)) as PlayerController;
        controller.TakePlayerControl();

        // Pause all of the monsters
        Monsters.Select(m => m.GetComponent(typeof(MonsterScript)) as MonsterScript)
            .ToList()
            .ForEach(ms => ms.IsPaused = true);
    }

    public void UnpauseGame()
    {
        // Revoke player control
        PlayerController controller = GetComponent(typeof(PlayerController)) as PlayerController;
        controller.GivePlayerControl();

        // Pause all of the monsters
        Monsters.Select(m => m.GetComponent(typeof(MonsterScript)) as MonsterScript)
            .ToList()
            .ForEach(ms => ms.IsPaused = false);
    }
}