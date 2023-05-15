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


        // combine the two positions into a vector3
        Vector3 newPosition = CheckIfValidSpawnLocation(randomXPosition, randomYPosition);

        return newPosition;
    }

    private Vector2 SpawnEnemyOnBottom()
    {
        // generate a random position anywhere on the x axis of the off-screen (left, right, or center)
        float randomXPosition = Random.Range(leftBounds, rightBounds);

        // generate a random position on the bottom portion of the off-screen
        float randomYPosition = Random.Range(bottomBounds - maximumYScreenOffset, bottomBounds - minimumYScreenOffset);


        // combine the two positions into a vector2
        Vector3 newPosition = CheckIfValidSpawnLocation(randomXPosition, randomYPosition);

        return newPosition;
    }


    private Vector2 SpawnEnemyOnRight()
    {
        // generate a random position on the right portion of the off-screen
        float randomXPosition = Random.Range(rightBounds + minimumXScreenOffset, rightBounds + maximumXScreenOffset);

        // generate a random position anywhere on the y axis of the off-screen (top, bottom, or center)
        float randomYPosition = Random.Range(bottomBounds, topBounds);

        // combine the two positions into a vector3
        Vector3 newPosition = CheckIfValidSpawnLocation(randomXPosition, randomYPosition);

        return newPosition;
    }

    private Vector2 SpawnEnemyOnLeft()
    {
        // generate a random position on the left portion of the off-screen
        float randomXPosition = Random.Range(leftBounds - minimumXScreenOffset, leftBounds - maximumXScreenOffset);

        // generate a random position anywhere on the y axis of the off-screen (top, bottom, or center)
        float randomYPosition = Random.Range(bottomBounds, topBounds);


        // combine the two positions into a vector3
        Vector3 newPosition = CheckIfValidSpawnLocation(randomXPosition, randomYPosition);//new Vector3(randomXPosition, randomYPosition);

        return newPosition;
    }

    public Vector2 PickRandomLocationOnMap()
    {
        Vector2 randomLocation = Vector2.zero;

        // pick a random number from 1-4
        int randomChoice = Random.Range(1, 5);

        // A value of 1 means return a spawn location at the top of the offscreen
        // 2 means return a spawn location at the bottom of the offscreen
        // 3 means return a spawn location at the right of the offscreen
        // 4 means return a spawn location at the left of the offscreen
        switch (randomChoice)
        {
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

    /// <summary>
    /// Take in an X position and Y position, then check if those values are outside of the map.
    /// If so, then call a function that will generate a new X and Y position that will not be outside of the map.
    /// * This will cause a stack overflow if the map is too small! So, adjust the screen boundaries and spawning offsets if the map should be small. *
    /// </summary>
    /// <param name="randomXPosition"></param>
    /// <param name="randomYPosition"></param>
    /// <returns></returns>
    public Vector2 CheckIfValidSpawnLocation(float randomXPosition, float randomYPosition)
    {
        // if spawning location's Y value is below the map, then generate a position above the player
        if (randomYPosition < surfaceBottomBoundary)
            return SpawnEnemyOnTop();

        else if (randomXPosition < surfaceLeftBoundary)
            return SpawnEnemyOnRight();

        else if (randomXPosition > surfaceRightBoundary)
            return SpawnEnemyOnLeft();

        else if (randomYPosition > surfaceTopBoundary)
            return SpawnEnemyOnBottom();

        return new Vector2(randomXPosition, randomYPosition);

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
    }
}
