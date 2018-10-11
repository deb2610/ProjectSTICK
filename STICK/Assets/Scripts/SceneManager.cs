using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneManager : MonoBehaviour {

    private static string mazeText =
        "01000000000000\r\n" +
        "01111100000000\r\n" +
        "01000111100000\r\n" +
        "01000000111110\r\n" +
        "01000000100000\r\n" +
        "01111111100000\r\n" +
        "00010000000000\r\n" +
        "00011110000000\r\n" +
        "00000100000000\r\n" +
        "00111111111100\r\n" +
        "00100000001000\r\n" +
        "00111111111000\r\n" +
        "00000000001110\r\n" +
        "00000000001000";

    public GameObject wallBlockPrefab;
    public GameObject player;
    public GameObject mainCamera;

    // Use this for initialization
	void Start () {
        // Build the wall!
        float wallWidth = wallBlockPrefab.transform.lossyScale.x;
        int[,] maze = readMaze(mazeText);
        Vector3 mazeOffset = new Vector3(-maze.GetLength(0) * wallWidth / 2, -maze.GetLength(1) * wallWidth / 2, 9.5f);
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for(int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == '0')
                {
                    Vector3 wallPos = new Vector3(i * wallWidth, j * wallWidth, 0) + mazeOffset;
                    Instantiate(wallBlockPrefab, wallPos, Quaternion.identity);
                }
            }
        }
        transform.Translate(new Vector3(-7f, -6.5f, 0));
        player.transform.Rotate(new Vector3(0, 0, -90f));
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private static int[,] readMaze(string stringMaze)
    {
        if (string.IsNullOrEmpty(stringMaze))
        {
            return null;
        }

        // Deal with unix/windows line endings
        stringMaze.Replace("\r", "");

        string[] rows = stringMaze.Split('\n');

        int[,] intMaze = new int[rows.Length, rows[0].Length];
        for(int i = 0; i < rows.Length; i++)
        {
            string row = rows[i];
            for( int j = 0; j < row.Length; j++)
            {
                if (j > row.Length)
                {
                    intMaze[i, j] = '0';
                }
                else
                {
                    intMaze[i, j] = row[j];
                }
            }
        }

        return intMaze;
    }
}
