using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will calculate the boundaries of the screen and spawn enemies slightly off-screen
// The point is to have enemies walk into the scene, without the player seeing the enemies spawn
public class OffScreenSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    
    [SerializeField] private int amountSpawned; // amount of enemies spawned 
    [Range(4, 150)]
    [SerializeField] private int maxAmountOfSpawn; // max amount of enemies this spawner can create TEMPORARY

    [SerializeField] private float timeUntilSpawn; // the time until spawning begins
    [SerializeField] private float timeBetweenSpawn; // the time between each enemy spawn
    [SerializeField] private GameObject enemyToSpawn; // the enemy to spawn in

    [Header("X Offsets")]
    [SerializeField] private float minimumXScreenOffset; // the minimum offset added to the right or left screen boundaries
    [SerializeField] private float maximumXScreenOffset; // the maximum offset added to the right or left screen boundaries

    [Header("Y Offsets")]
    [SerializeField] private float minimumYScreenOffset; // the minimum offset added to the top or bottom screen boundaries
    [SerializeField] private float maximumYScreenOffset; // the maximum offset added to the top or bottom screen boundaries


    private float rightBounds; // starting point for the right portion of the off-screen
    private float topBounds; // starting point for the top portion of the off-screen
    private float leftBounds; // starting point for the left portion of the off-screen
    private float bottomBounds; // starting point for the bottom portion of the off-screen


    private Vector3 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        // calculates screen boundaries (this is so that we can spawn enemies off screen)
        // when the player's transform position is at (0,0)...
        // the rightBounds start at 26, and the leftBounds start at -26
        // the topBounds start at 15, and the bottomBounds start at -15
        //screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // HARD-CODED VERSION
        screenBounds = new Vector2(26, 15);

        // rightBounds is initialized to the screenBounds.x when the game starts
        rightBounds = screenBounds.x;
        // leftBounds is initialized to the -screenBounds.x when the game starts
        leftBounds = -screenBounds.x;
        // topBounds is initialized to the screenBounds.y when the game starts
        topBounds = screenBounds.y;
        // bottomBounds is initialized to the -screenBounds.y when the game starts
        bottomBounds = -screenBounds.y;


        // does not check when enemy was killed * TEMPORARY
        //if(amountSpawned < maxAmountOfSpawn - 4)
        //{
        //    InvokeRepeating(nameof(SpawnEnemyOnTop), timeUntilSpawn, timeBetweenSpawn);
        //    InvokeRepeating(nameof(SpawnEnemyOnBottom), timeUntilSpawn, timeBetweenSpawn);
        //    InvokeRepeating(nameof(SpawnEnemyOnRight), timeUntilSpawn, timeBetweenSpawn);
        //    InvokeRepeating(nameof(SpawnEnemyOnLeft), timeUntilSpawn, timeBetweenSpawn);
        //}
    }

    void SpawnEnemyOnTop()
    {
        // generate a random position anywhere on the x axis of the off-screen (left, right, or center)
        float randomXPosition = Random.Range(leftBounds, rightBounds);

        // generate a random position on the top portion of the off-screen
        float randomYPosition = Random.Range(topBounds + minimumYScreenOffset, topBounds + maximumYScreenOffset);

        // combine the two positions into a vector3
        Vector3 newPosition = new Vector3(randomXPosition, randomYPosition);

        // spawn the enemy at the newPosition
        Instantiate(enemyToSpawn, newPosition, Quaternion.identity);

        amountSpawned++;
    }

    void SpawnEnemyOnBottom()
    {
        // generate a random position anywhere on the x axis of the off-screen (left, right, or center)
        float randomXPosition = Random.Range(leftBounds, rightBounds);

        // generate a random position on the bottom portion of the off-screen
        float randomYPosition = Random.Range(bottomBounds - maximumYScreenOffset, bottomBounds - minimumYScreenOffset);

        // combine the two positions into a vector3
        Vector3 newPosition = new Vector3(randomXPosition, randomYPosition);

        // spawn the enemy at the newPosition
        Instantiate(enemyToSpawn, newPosition, Quaternion.identity);

        amountSpawned++;
    }

    void SpawnEnemyOnRight()
    {
        // generate a random position on the right portion of the off-screen
        float randomXPosition = Random.Range(rightBounds + minimumXScreenOffset, rightBounds + maximumXScreenOffset);

        // generate a random position anywhere on the y axis of the off-screen (top, bottom, or center)
        float randomYPosition = Random.Range(bottomBounds, topBounds);

        // combine the two positions into a vector3
        Vector3 newPosition = new Vector3(randomXPosition, randomYPosition);

        // spawn the enemy at the newPosition
        Instantiate(enemyToSpawn, newPosition, Quaternion.identity);

        amountSpawned++;
    }

    void SpawnEnemyOnLeft()
    {
        // generate a random position on the left portion of the off-screen
        float randomXPosition = Random.Range(leftBounds - minimumXScreenOffset, leftBounds - maximumXScreenOffset);

        // generate a random position anywhere on the y axis of the off-screen (top, bottom, or center)
        float randomYPosition = Random.Range(bottomBounds, topBounds);

        // combine the two positions into a vector3
        Vector3 newPosition = new Vector3(randomXPosition, randomYPosition);

        // spawn the enemy at the newPosition
        Instantiate(enemyToSpawn, newPosition, Quaternion.identity);

        amountSpawned++;
    }

    // Update is called once per frame
    void Update()
    {
        // Because the player is moving all around, the boundaries of the screen must update relative to their position

        // Think of it like this... if the player is moving far to the left, then the left and right boundaries of the camera
        // must also move left (camera is moving left with the player), meaning the leftBound and rightBound values would become smaller

        // Think of it like this as well... if the player is moving far to the right, then the left and right boundaries of the camera
        // must also move right, meaning the leftBound and rightBound values would become larger

        if(amountSpawned < maxAmountOfSpawn)
        {
            SpawnEnemyOnTop();
            SpawnEnemyOnLeft();
            SpawnEnemyOnRight();
            SpawnEnemyOnBottom();
        }

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
