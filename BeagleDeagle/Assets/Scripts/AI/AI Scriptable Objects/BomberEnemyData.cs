using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewBomberEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/BomberEnemy")]
public class BomberEnemyData : EnemyData
{
    [Space(25f)] 
    
    [RestrictedPrefab(typeof(IExplosiveUpdatable))]
    public GameObject explosivePrefab;

    public ExplosiveData explosiveData;
    
    [FormerlySerializedAs("explosiveType")] public StatusEffectTypes statusEffects;

    // How long will the explosive gameObject remain (meant for explosives with AOEs)
    [Range(0.1f, 30f)] public float explosiveDuration;
}
