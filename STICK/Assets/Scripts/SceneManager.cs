using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneManager : MonoBehaviour {

    // A readonly singly initialized property to expose the maze to other entitites 
    private char[,] mazeArray;
    /// <summary>
    /// Contains an array of characters representing the tiles in a maze:
    /// Currently, the tiles are represented by the following characters:
    /// - '0' : The Wall of the maze, visualized in game as tree
    /// - '1' : An empty space in the maze (Invalid characters default to this)
    /// </summary>
    public char[,] MazeArray
    {
        get
        {
            return mazeArray ?? (mazeArray = ReadMaze());
        }
    }
    public List<GameObject> Monsters { get; private set; }
    public GameObject wallBlockPrefab;
    public GameObject player;
    public GameObject mainCamera;
    public TextAsset mazeFile;
    public GameObject monsterPrefab;
    public int numberOfMonsters = 10;
    private Vector3 mazeOffset;

    // Use this for initialization
	void Start () {
        // Build the wall!
        float wallWidth = wallBlockPrefab.transform.lossyScale.x;
        mazeOffset = new Vector3(-MazeArray.GetLength(0) * wallWidth / 2, -MazeArray.GetLength(1) * wallWidth / 2, 9.5f);
        for (int i = 0; i < MazeArray.GetLength(0); i++)
        {
            for(int j = 0; j < MazeArray.GetLength(1); j++)
            {
                if (MazeArray[i, j] == '0')
                {
                    Vector3 wallPos = new Vector3(i * wallWidth, j * wallWidth, 0) + mazeOffset;
                    Instantiate(wallBlockPrefab, wallPos, Quaternion.identity);
                }
            }
        }
        transform.Translate(new Vector3(-7f, -6.5f, 0));
        player.transform.Rotate(new Vector3(0, 0, -90f));

        // Spawn some spooky bois
        SpawnMonsters();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private char[,] ReadMaze()
    {
        string stringMaze = mazeFile.text;
        if (string.IsNullOrEmpty(stringMaze))
        {
            return null;
        }

        // Deal with unix/windows line endings
        stringMaze.Replace("\r", "");

        string[] rows = stringMaze.Split('\n');

        char[,] charMaze = new char[rows.Length, rows[0].Length];
        for(int i = 0; i < rows.Length; i++)
        {
            string row = rows[i];
            for( int j = 0; j < row.Length; j++)
            {
                if (j > row.Length)
                {
                    charMaze[i, j] = '0';
                }
                else
                {
                    charMaze[i, j] = row[j];
                }
            }
        }

        return charMaze;
    }

    /// <summary>
    /// Creates a bunch of monsters throughout the maze
    /// </summary>
    private void SpawnMonsters()
    {
        int monstersGenerated = 0;
        float wallWidth = wallBlockPrefab.transform.lossyScale.x;
        Monsters = new List<GameObject>();
        while (monstersGenerated < numberOfMonsters)
        {
            // Pick a random tile
            int nextX = (int)(Random.value * MazeArray.GetLength(0));
            int nextY = (int)(Random.value * MazeArray.GetLength(1));

            // Check for empty tile
            if(MazeArray[nextX, nextY] == '1')
            {
                Vector3 position = new Vector3(nextX * wallWidth, nextY * wallWidth, 0) + mazeOffset;
                GameObject newMonster = Instantiate(monsterPrefab, position, Quaternion.identity);
                newMonster.SetActive(false);
                Monsters.Add(newMonster);
                monstersGenerated++;
            }
        }
    }
}
