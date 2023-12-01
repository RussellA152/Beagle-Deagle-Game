using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /* EnemyManager is not responsible for spawning enemies, it only listens for enemies spawning.
     The ObjectPooler spawns enemies and other reused gameObjects. */

    public static EnemyManager Instance;

    public event Action<GameObject> onEnemyInstantiated;
    
    // Pass the gameObject of the enemy that died
    public event Action<GameObject> onEnemyDeathGiveGameObject;

    private readonly List<GameObject> _allEnemyGameObjects = new List<GameObject>();
    private readonly List<IHasTarget> _allEnemiesWithTargets = new List<IHasTarget>();

    [SerializeField] private Transform _globalTarget;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Target is almost always the player (at the start of the game)
        _globalTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDisable()
    {
        _allEnemyGameObjects.Clear();
    }

    ///-///////////////////////////////////////////////////////////
    /// Add a new enemy to a list of every enemy gameObject that has been instantiated, then set
    /// their targeted transform to whoever is the "globalTarget."
    public void RegisterNewEnemy(GameObject newEnemy)
    {
        _allEnemyGameObjects.Add(newEnemy);

        IHasTarget targetingScript = newEnemy.GetComponent<IHasTarget>();
        
        _allEnemiesWithTargets.Add(targetingScript);
        
        targetingScript.SetNewTarget(_globalTarget);
        
        InvokeEnemyWasInstantiated(newEnemy);
    }

    ///-///////////////////////////////////////////////////////////
    /// Tell all enemies that they have a new object to follow and attack
    /// 
    public void ChangeAllEnemyTarget(Transform newTarget)
    {
        foreach (IHasTarget enemyWithTarget in _allEnemiesWithTargets)
        {
            enemyWithTarget.SetNewTarget(newTarget);
        }

        _globalTarget = newTarget;
    }

    public Transform GetGlobalTarget()
    {
        return _globalTarget;
    }

    private void InvokeEnemyWasInstantiated(GameObject enemyThatInstantiated)
    {
        onEnemyInstantiated?.Invoke(enemyThatInstantiated);
    }

    ///-///////////////////////////////////////////////////////////
    /// When an enemy is killed, pass their gameObject to other scripts
    /// that need a reference.
    /// 
    public void InvokeEnemyDeathGiveGameObject(GameObject enemyGameObject)
    {
        onEnemyDeathGiveGameObject?.Invoke(enemyGameObject);
    }
    
}
