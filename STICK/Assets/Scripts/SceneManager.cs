﻿using System.Collections;
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
    public GameObject goal;
    public TextAsset mazeFile;
    private Vector3 mazeOffset;
    private float mazeScale; // Represents the width in Unity units of one maze block
    private Vector3 startIndex;

    // Use this for initialization
    void Start()
    {
        // Build the wall!
        mazeScale = wallBlockPrefab.transform.lossyScale.x;
        mazeOffset = new Vector3(-MazeArray.GetLength(0) * mazeScale / 2, -MazeArray.GetLength(1) * mazeScale / 2, 9.5f);
        startIndex = new Vector3(0, 1, 0);
        for (int i = 0; i < MazeArray.GetLength(0); i++)
        {
            for (int j = 0; j < MazeArray.GetLength(1); j++)
            {
                switch(MazeArray[i, j])
                {
                    case '0':
                        Vector3 wallPos = new Vector3(i * mazeScale, j * mazeScale, 0) + mazeOffset;
                        Instantiate(wallBlockPrefab, wallPos, Quaternion.identity);
                        break;
                    case 'S':
                        startIndex = new Vector3(i, j, 0);
                        break;
                    case 'E':
                        Vector3 goalPos = new Vector3(i * mazeScale, j * mazeScale, 0.15f) + mazeOffset;
                        goal.transform.position = goalPos;
                        break;
                }
                if (MazeArray[i, j] == '0')
                {
                    Vector3 wallPos = new Vector3(i * mazeScale, j * mazeScale, 0) + mazeOffset;
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

    public Vector3Int GlobalToMazeCoordinate(Vector3 coordinate)
    {
        Vector3 shiftedOrigin = (coordinate - mazeOffset) / mazeScale;
        return Vector3Int.FloorToInt(shiftedOrigin);
    }

    public bool IsPointInMazeBlock(Vector3 coordinate)
    {
        Vector3Int indeces = GlobalToMazeCoordinate(coordinate);
        // Check outside the maze boundaries
        if (indeces.x < 0 || indeces.x >= mazeArray.GetLength(0)
            || indeces.y < 0 || indeces.y >= mazeArray.GetLength(1))
        {
            return false;
        }
        char mazeBlock = mazeArray[indeces.x, indeces.y];
        return mazeBlock == '0';
    }
}