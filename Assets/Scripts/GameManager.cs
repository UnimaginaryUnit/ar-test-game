﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct GameState 
{
    public string stage;
    public List<GameObject> obstacles;

    public GameState(string stage) 
    {
        this.stage = stage;
        obstacles = new List<GameObject>();
    }
}

public class GameManager : MonoBehaviour
{
    // Prefabs
    [SerializeField]
    private GameObject[] cubes = new GameObject[5];

    // Gameobjects
    public GameObject lockButton;
    public GameObject startButton;
    public GameObject obstaclesRoot;
    public InteractionManager interactionManager;

    // State
    private GameState state = new GameState("notStarted");

    // Variables
    private float obstacleSpawnDelay = 4f; // sec
    private float obstacleSpeed = 0.5f; // m/sec
    private float lastObstacleSpawned = 0;

    public string getState() 
    {
        return state.stage;
    }

    public void startGame() 
    {
        if(state.stage == "notStarted" && interactionManager.getPlacementPoseLocked()) 
        {
            state.stage = "started";
        }
    }

    void Update() 
    {   
        if(state.stage == "started") 
        {
            // Move spawned obstacles to the player
            moveObstacles();
            // Check if the player hit any (or moved away from the playfield)
            checkCollision();
            // Delete obstacles behind the player
            deleteObstacles();
            // Spawn new obstacles
            spawnObstacles();
        }

        // Update UI from the gamestate
        updateFromState();
    }

    private void updateFromState()
    {
        if(state.stage == "started") 
        {
            lockButton.SetActive(false);
            startButton.SetActive(false);
        }
        else
        {
            lockButton.SetActive(true);
            lockButton.SetActive(true);
        }
    }

    private void checkCollision()
    {
        // TODO: finish
        return;
    }

    private void generateObstacle()
    {
        // Obstacle starting cell
        int obstacleStart = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
        // Length of obstacle (amount of cells)
        int obstaclelength = Mathf.Min(Mathf.RoundToInt(UnityEngine.Random.Range(1f, 4f)), (5 - obstacleStart));

        for(var i = obstacleStart; i < obstacleStart + obstaclelength; ++i) {
            state.obstacles.Add(Instantiate(cubes[i], obstaclesRoot.transform));
        }
    }

    private void spawnObstacles()
    {
        lastObstacleSpawned += Time.deltaTime;
        Debug.Log(lastObstacleSpawned);
        if(lastObstacleSpawned > obstacleSpawnDelay) 
        {
            Debug.Log("OBSTACLE SPAWNED");
            lastObstacleSpawned = 0;
            generateObstacle();
        }
    }

    private void deleteObstacles()
    {
        foreach(var obstacle in state.obstacles) {
            if(obstacle.transform.localPosition.z > 3)
            {
                state.obstacles.Remove(obstacle);
                Destroy(obstacle);
            }
        }
    }

    private void moveObstacles()
    {
        foreach(var obstacle in state.obstacles) 
        {
            var currentPosition = obstacle.transform.localPosition;
            currentPosition.z += Time.deltaTime * obstacleSpeed;
            obstacle.transform.localPosition = currentPosition;
        }
    }
}
