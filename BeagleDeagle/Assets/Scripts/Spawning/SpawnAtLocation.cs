using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtLocation : MonoBehaviour
{
    [SerializeField]
    private int amountSpawned;

    [SerializeField] private float timeBetweenSpawn; // the time between each enemy spawn

    [SerializeField] private GameObject enemyToSpawn; // the enemy to spawn in

    [SerializeField]
    private float spawnRange;

    [SerializeField]
    private LayerMask whatTriggersSpawn;

    private bool inSpawnRange;

    private bool canSpawn = true;

    private void Start()
    {
        amountSpawned = 0;
    }
    private void Update()
    {
        inSpawnRange = Physics2D.OverlapCircle(transform.position, spawnRange, whatTriggersSpawn);

        if (canSpawn && inSpawnRange)
        {
            SpawnAnEnemy();

            StartCoroutine(SpawnCooldown());
        }
    }

    private void SpawnAnEnemy()
    {
        // combine the two positions into a vector3
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y);

        // spawn the enemy at the newPosition
        Instantiate(enemyToSpawn, newPosition, Quaternion.identity);

        amountSpawned++;
    }

    IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(timeBetweenSpawn);
        canSpawn = true;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRange);

    }
}
