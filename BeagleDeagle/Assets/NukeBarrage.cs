using System.Collections;
using UnityEngine;

public class NukeBarrage : Nuke
{
    [SerializeField] private GameObject extraNukeGameObject;

    [SerializeField, Range(0.1f, 20f)] 
    private float spawnOffset;

    [SerializeField, Range(0.1f, 20f)] 
    private float delayBetweenSpawn = 0.5f;
    
    public override void Activate(Vector2 aimDirection)
    {
        base.Activate(aimDirection);

        StartCoroutine(SpawnExtraNukesWithDelay());
    }

    ///-///////////////////////////////////////////////////////////
    /// Spawn 4 nukes (with a delay in between) at each corner of this gameObject.
    /// 
    private IEnumerator SpawnExtraNukesWithDelay()
    {
        yield return new WaitForSeconds(delayBetweenSpawn); // Initial delay, adjust as needed

        // Top right spawn
        CreateExtraNuke(new Vector2(1, 1));
        yield return new WaitForSeconds(delayBetweenSpawn);

        // Top left spawn
        CreateExtraNuke(new Vector2(-1, 1));
        yield return new WaitForSeconds(delayBetweenSpawn);

        // Bottom right spawn
        CreateExtraNuke(new Vector2(1, -1));
        yield return new WaitForSeconds(delayBetweenSpawn);

        // Bottom left spawn
        CreateExtraNuke(new Vector2(-1, -1));
    }

    ///-///////////////////////////////////////////////////////////
    /// Spawn a nuke child gameObject at a position local to this nuke gameObject.
    /// 
    private void CreateExtraNuke(Vector2 position)
    {
        Vector2 spawnPosition = new Vector2((position.x * spawnOffset) + transform.position.x, (position.y * spawnOffset) + transform.position.y);
        
        GameObject extraNuke = Instantiate(extraNukeGameObject, transform);
        extraNuke.SetActive(false);

        IExplosiveUpdatable explosiveScript = extraNuke.GetComponent<IExplosiveUpdatable>();
           
        // Give the nuke the same stats as this one
        explosiveScript.SetDamage(Damage);
        explosiveScript.SetDuration(Duration);
        explosiveScript.UpdateScriptableObject(ExplosiveData);
            
        extraNuke.SetActive(true);
        extraNuke.GetComponentInChildren<NukeShadowAnimation>().PlayNukeShadowAnimation();
        
        explosiveScript.Activate(spawnPosition);
    }
}