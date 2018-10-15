using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneManager : MonoBehaviour
{

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

    public GameObject wallBlockPrefab;
    public GameObject player;
    public GameObject mainCamera;
    public TextAsset mazeFile;
    private Vector3 mazeOffset;

    private Vector3 startIndex;

    // Use this for initialization
    void Start()
    {
        // Build the wall!
        float wallWidth = wallBlockPrefab.transform.lossyScale.x;
        mazeOffset = new Vector3(-MazeArray.GetLength(0) * wallWidth / 2, -MazeArray.GetLength(1) * wallWidth / 2, 9.5f);
        Debug.Log("Maze offset: " + mazeOffset);
        startIndex = new Vector3(0, 1, 0);
        for (int i = 0; i < MazeArray.GetLength(0); i++)
        {
            for (int j = 0; j < MazeArray.GetLength(1); j++)
            {
                switch(MazeArray[i, j])
                {
                    case '0':
                        Vector3 wallPos = new Vector3(i * wallWidth, j * wallWidth, 0) + mazeOffset;
                        Instantiate(wallBlockPrefab, wallPos, Quaternion.identity);
                        break;
                    case 'S':
                        startIndex = new Vector3(i, j, 0);
                        Debug.Log("Set start index: " + startIndex);
                        break;

                }
                if (MazeArray[i, j] == '0')
                {
                    Vector3 wallPos = new Vector3(i * wallWidth, j * wallWidth, 0) + mazeOffset;
                    Instantiate(wallBlockPrefab, wallPos, Quaternion.identity);
                }
            }
        }

        transform.position = (startIndex + mazeOffset);
        player.transform.Rotate(new Vector3(0, 0, -90f));
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log("Creating List");
        char[,] charMaze = new char[rows.Length, rows[0].Length];
        for (int i = 0; i < rows.Length; i++)
        {
            string row = rows[i];
            for (int j = 0; j < row.Length; j++)
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
}