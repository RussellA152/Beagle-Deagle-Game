using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script will calculate the boundaries of the screen and spawn enemies slightly off-screen
// The point is to have enemies walk into the scene, without the player seeing the enemies spawn
public class OffScreenSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Header("Camera Bounds (NEED TO SCALE SCALE WITH MAP SIZE)")]
    [SerializeField]
    private Vector3 screenBounds = new Vector2(17, 17); // this is the boundaries of the camera, (17,17) is a good value, but this value NEEDS TO BE LOWER if the map is too small

    [Header("The Map of This Level")]
    [SerializeField]
    private NavMeshSurface2d mapSurface;

    [Header("Boundaries of Map")]
    [SerializeField]
    private float surfaceTopBoundary; // the top boundary of the navmesh surface (i.e the walkable area for enemies)
    [SerializeField]
    private float surfaceBottomBoundary; // the bottom boundary of the navmesh surface
    [SerializeField]
    private float surfaceLeftBoundary; // the left boundary of the navmesh surface
    [SerializeField]
    private float surfaceRightBoundary; // the right boundary of the navmesh surface

    [Header("X Enemy Spawning Offsets")]
    // These offsets are applied to the location that the enemy spawns at
    // This is so that enemies do not spawn at the exact location of each other
    [SerializeField] private float minimumXScreenOffset; // the minimum offset ADDED to the right or left screen boundaries
    [SerializeField] private float maximumXScreenOffset; // the maximum offset ADDED to the right or left screen boundaries

    [Header("Y Enemy Spawning Offsets")]
    [SerializeField] private float minimumYScreenOffset; // the minimum offset ADDED to the top or bottom screen boundaries
    [SerializeField] private float maximumYScreenOffset; // the maximum offset ADDED to the top or bottom screen boundaries


    private float rightBounds; // starting point for the right portion of the off-screen
    private float topBounds; // starting point for the top portion of the off-screen
    private float leftBounds; // starting point for the left portion of the off-screen
    private float bottomBounds; // starting point for the bottom portion of the off-screen

    [SerializeField]
    private bool playerCloseToLeftBoundary;
    [SerializeField]
    private bool playerCloseToRightBoundary;
    [SerializeField]
    private bool playerCloseToTopBoundary;
    [SerializeField]
    private bool playerCloseToBottomBoundary;


    // Start is called before the first frame update
    void Start()
    {
        // rightBounds is initialized to the screenBounds.x when the game starts
        rightBounds = screenBounds.x;
        // leftBounds is initialized to the -screenBounds.x when the game starts
        leftBounds = -screenBounds.x;
        // topBounds is initialized to the screenBounds.y when the game starts
        topBounds = screenBounds.y;
        // bottomBounds is initialized to the -screenBounds.y when the game starts
        bottomBounds = -screenBounds.y;

        // calculating all boundaries of the map (this is so that we can check if an enemy is about to spawn outside of the map)
        surfaceTopBoundary = mapSurface.navMeshData.sourceBounds.max.z;
        surfaceBottomBoundary = mapSurface.navMeshData.sourceBounds.min.z;
        surfaceLeftBoundary = mapSurface.navMeshData.sourceBounds.min.x;
        surfaceRightBoundary = mapSurface.navMeshData.sourceBounds.max.x;

    }

    private Vector2 SpawnEnemyOnTop()
    {
        // generate a random position anywhere on the x axis of the off-screen (left, right, or center)
        float randomXPosition = Random.Range(leftBounds, rightBounds);

        // generate a random position on the top portion of the off-screen
        float randomYPosition = Random.Range(topBounds + minimumYScreenOffset, topBounds + maximumYScreenOffset);

        randomXPosition = Mathf.Clamp(randomXPosition, surfaceLeftBoundary, surfaceRightBoundary);

        randomYPosition = Mathf.Clamp(randomYPosition, surfaceBottomBoundary,surfaceTopBoundary);

        return new Vector2(randomXPosition, randomYPosition);
    }

    private Vector2 SpawnEnemyOnBottom()
    {
        // generate a random position anywhere on the x axis of the off-screen (left, right, or center)
        float randomXPosition = Random.Range(leftBounds, rightBounds);

        // generate a random position on the bottom portion of the off-screen
        float randomYPosition = Random.Range(bottomBounds - maximumYScreenOffset, bottomBounds - minimumYScreenOffset);

        randomXPosition = Mathf.Clamp(randomXPosition, surfaceLeftBoundary, surfaceRightBoundary);

        randomYPosition = Mathf.Clamp(randomYPosition, surfaceBottomBoundary, surfaceTopBoundary);

        return new Vector2(randomXPosition, randomYPosition);
    }


    private Vector2 SpawnEnemyOnRight()
    {
        // generate a random position on the right portion of the off-screen
        float randomXPosition = Random.Range(rightBounds + minimumXScreenOffset, rightBounds + maximumXScreenOffset);

        // generate a random position anywhere on the y axis of the off-screen (top, bottom, or center)
        float randomYPosition = Random.Range(bottomBounds, topBounds);

        randomXPosition = Mathf.Clamp(randomXPosition, surfaceLeftBoundary, surfaceRightBoundary);

        randomYPosition = Mathf.Clamp(randomYPosition, surfaceBottomBoundary, surfaceTopBoundary);

        return new Vector2(randomXPosition, randomYPosition);
    }

    private Vector2 SpawnEnemyOnLeft()
    {
        // generate a random position on the left portion of the off-screen
        float randomXPosition = Random.Range(leftBounds - minimumXScreenOffset, leftBounds - maximumXScreenOffset);

        // generate a random position anywhere on the y axis of the off-screen (top, bottom, or center)
        float randomYPosition = Random.Range(bottomBounds, topBounds);

        randomXPosition = Mathf.Clamp(randomXPosition, surfaceLeftBoundary, surfaceRightBoundary);

        randomYPosition = Mathf.Clamp(randomYPosition, surfaceBottomBoundary, surfaceTopBoundary);

        return new Vector2(randomXPosition, randomYPosition);
    }

    public Vector2 PickRandomLocationOnMap()
    {
        Vector2 randomLocation = Vector2.zero;

        // pick a random number from 1-4
        //int randomChoice = Random.Range(1, 5);
        int randomChoice = GenerateRandomChoice();

        // A value of 1 means return a spawn location at the top of the offscreen
        // 2 means return a spawn location at the bottom of the offscreen
        // 3 means return a spawn location at the right of the offscreen
        // 4 means return a spawn location at the left of the offscreen
        switch (randomChoice)
        {
            case 0:
                Debug.Log("Error. Enemy could not spawn!");
                break;
            case 1:
                randomLocation = SpawnEnemyOnTop();
                break;
            case 2:
                randomLocation = SpawnEnemyOnBottom();
                break;
            case 3:
                randomLocation = SpawnEnemyOnRight();
                break;
            case 4:
                randomLocation = SpawnEnemyOnLeft();
                break;
        }
        return randomLocation;
    }

    private int GenerateRandomChoice()
    {
        int randomChoice = 0;
        int subRandomChoice = 0;
        if (playerCloseToTopBoundary)
        {
            if (playerCloseToLeftBoundary)
            {
                subRandomChoice = Random.Range(1, 3);

                if (subRandomChoice == 1)
                    randomChoice = 2;
                else
                    randomChoice = 3;
            }
            else if (playerCloseToRightBoundary)
            {
                subRandomChoice = Random.Range(1, 3);

                if (subRandomChoice == 1)
                    randomChoice = 2;
                else
                    randomChoice = 4;
            }
            else
            {
                randomChoice = Random.Range(2, 5);
            }
        }
        else if (playerCloseToBottomBoundary)
        {
            if (playerCloseToLeftBoundary)
            {
                subRandomChoice = Random.Range(1, 3);

                if (subRandomChoice == 1)
                    randomChoice = 1;
                else
                    randomChoice = 3;
            }
            else if (playerCloseToRightBoundary)
            {
                subRandomChoice = Random.Range(1, 3);

                if (subRandomChoice == 1)
                    randomChoice = 1;
                else
                    randomChoice = 4;
            }
            else
            {
                subRandomChoice = Random.Range(1, 4);

                if (subRandomChoice == 1)
                    randomChoice = 1;
                else if(subRandomChoice == 2)
                    randomChoice = 3;
                else if (subRandomChoice == 3)
                    randomChoice = 4;
            }
        }
        else if (playerCloseToRightBoundary)
        {
            subRandomChoice = Random.Range(1, 4);

            if (subRandomChoice == 1)
                randomChoice = 1;
            else if (subRandomChoice == 2)
                randomChoice = 2;
            else if (subRandomChoice == 3)
                randomChoice = 4;
        }
        else if (playerCloseToLeftBoundary)
        {
            subRandomChoice = Random.Range(1, 4);

            if (subRandomChoice == 1)
                randomChoice = 1;
            else if (subRandomChoice == 2)
                randomChoice = 2;
            else if (subRandomChoice == 3)
                randomChoice = 3;
        }
        else if(playerCloseToTopBoundary && playerCloseToBottomBoundary && playerCloseToLeftBoundary && playerCloseToRightBoundary)
        {
            Debug.Log("Player is close to all corners! Map is too small!");
            return 0;
        }

        else
        {
            randomChoice = Random.Range(1, 5);
        }

        return randomChoice;

        
    }


    // Checking if the player is close to the boundaries of the map
    private void CheckPlayerProximityToBoundaries()
    {
        if (playerTransform.position.x <= surfaceLeftBoundary + maximumXScreenOffset)
        {
            playerCloseToLeftBoundary = true;
        }

        else
        {
            playerCloseToLeftBoundary = false;

        }


        if (playerTransform.position.x >= surfaceRightBoundary - maximumXScreenOffset)
        {
            playerCloseToRightBoundary = true;

        }

        else
        {
            playerCloseToRightBoundary = false;

        }


        if (playerTransform.position.y >= surfaceTopBoundary - maximumYScreenOffset)
        {
            playerCloseToTopBoundary = true;
        }

        else
        {
            playerCloseToTopBoundary = false;

        }


        if (playerTransform.position.y <= surfaceBottomBoundary + maximumYScreenOffset)
        {
            playerCloseToBottomBoundary = true;
        }

        else
        {
            playerCloseToBottomBoundary = false;

        }

    }



    // Update is called once per frame
    void Update()
    {
        // Because the player is moving all around, the boundaries of the screen must update relative to their position

        // Think of it like this -> if the player is moving far to the left, then the left and right boundaries of the camera
        // must also move left (camera is moving left with the player), meaning the leftBound and rightBound values would become smaller

        // Think of it like this as well -> if the player is moving far to the right, then the left and right boundaries of the camera
        // must also move right, meaning the leftBound and rightBound values would become larger

        if (playerTransform.position.x >= 0)
        {
            // rightBounds and topBounds are getting larger
            rightBounds = screenBounds.x + playerTransform.position.x;
            topBounds = screenBounds.y + playerTransform.position.y;

            // leftBounds are getting larger
            leftBounds = -screenBounds.x + playerTransform.position.x;
            bottomBounds = -screenBounds.y + playerTransform.position.y;
        }
        // If the player's x transform is less than 0...
        else if (playerTransform.position.x < 0)
        {
            // rightBounds will be getting smaller, because the player is moving in the negative side of the x-axis
            rightBounds = screenBounds.x + playerTransform.position.x;
            topBounds = screenBounds.y + playerTransform.position.y;

            // leftBounds will be getting smaller / the negative value is higher
            leftBounds = -1 * (screenBounds.x - playerTransform.position.x);
            bottomBounds = -1 * (screenBounds.y - playerTransform.position.y);
        }

        CheckPlayerProximityToBoundaries();
    }
}
