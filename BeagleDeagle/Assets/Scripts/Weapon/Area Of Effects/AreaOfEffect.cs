using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AreaOfEffect : MonoBehaviour, IHasMiscellaneousModifier
{
    protected AreaOfEffectData areaOfEffectData;

    [SerializeField, RestrictedPrefab(typeof(PoolableParticle))] 
    private GameObject areaOfEffectParticleEffect;

    private PoolableParticle _particleUsed;

    private int _particlePoolKey;
 
    private CheckObstruction _obstructionScript;
    private AudioClipPlayer _audioClipPlayer;

    private Outliner _outliner;
    
    private CapsuleCollider2D _triggerCollider;

    private float _bonusAoeSize = 1f;
    
    
    // The layer mask for walls
    private int _wallLayerMask;

    [SerializeField] 
    private UnityEvent<GameObject> onAreaEnter;
    
    [SerializeField] 
    private UnityEvent<GameObject> onAreaStay;
    
    [SerializeField] 
    private UnityEvent<GameObject> onAreaExit;


    private void Awake()
    {
        _triggerCollider = GetComponent<CapsuleCollider2D>();

        _outliner = GetComponent<Outliner>();

        _obstructionScript = GetComponentInParent<CheckObstruction>();

        _audioClipPlayer = GetComponentInParent<AudioClipPlayer>();
        
        if(areaOfEffectParticleEffect != null)
            _particlePoolKey = areaOfEffectParticleEffect.GetComponent<IPoolable>().PoolKey;

    }

    private void Start()
    {
        _wallLayerMask = LayerMask.GetMask("Wall");

    }
    

    private void OnDisable()
    {
        if (_particleUsed != null)
        {
            _particleUsed.StopAllParticles();
        }

        ResetMiscellaneousBonuses();

    }

    private void OnDestroy()
    {
        // Resetting the size of the trigger collider when destroyed
        _triggerCollider.size = new Vector2(1f, 1f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this AOE hits what its allowed to affect
        if ((areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Target has entered AOE, add them to overlappingTargets dictionary
            AreaOfEffectManager.Instance.AddNewOverlappingTarget(areaOfEffectData, collision.gameObject);
            
            onAreaEnter.Invoke(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Target has left AOE, so remove them from overlappingTarget dictionary
            // Also, they are no longer affected by their AOE, so remove them from affectedTargets dictionary (* Not exactly the case for DOT's *)
            // DOT's will also remove from affectedTargets if they had to reapply their DOT
            AreaOfEffectManager.Instance.RemoveOverlappingTarget(areaOfEffectData, collision.gameObject);

            // If the target is not standing in any AOE at all, then call OnAreaExit()
            if (!AreaOfEffectManager.Instance.IsTargetOverlappingAreaOfEffect(areaOfEffectData, collision.gameObject) && AreaOfEffectManager.Instance.CheckIfTargetIsAffected(areaOfEffectData, collision.gameObject))
            {
                // Only remove effect if this type of AOE is required to
                
                onAreaExit.Invoke(collision.gameObject);

                AreaOfEffectManager.Instance.RemoveTargetFromAffectedHashSet(areaOfEffectData, collision.gameObject);
                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If this AOE hits what its allowed to affect
        if ((areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // If the target is not obstructed by a wall, and they are not already affected by the AOE's ability (smoke or radiation), then call OnAreaStay()
            if (!_obstructionScript.HasObstruction(transform.position, collision.gameObject, _wallLayerMask)  && !AreaOfEffectManager.Instance.CheckIfTargetIsAffected(areaOfEffectData, collision.gameObject))
            {
                AreaOfEffectManager.Instance.TryAddAffectedTarget(areaOfEffectData, collision.gameObject);

                //PlayParticleEffect(collision.gameObject);
                
                onAreaStay.Invoke(collision.gameObject);
            }
        }
    }

    public void OnAreaOfEffectActivate()
    {
        if (areaOfEffectParticleEffect != null)
        {
            GameObject newParticleEffect = ObjectPooler.Instance.GetPooledObject(_particlePoolKey);
            _particleUsed = newParticleEffect.GetComponent<PoolableParticle>();
        
            _particleUsed.PlaceParticleOnTransform(transform);

            _particleUsed.PlayAllParticles(areaOfEffectData.aoeSpreadSize.x * _bonusAoeSize);
        }
        
    }

    public void PlayAreaofEffectSound(float duration)
    {
        if (areaOfEffectData.activationSound != null)
        {
            _audioClipPlayer.PlayForDurationAudioClip(areaOfEffectData.activationSound, areaOfEffectData.activationSoundVolume, duration);
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Changes the AOE effect of this instance
    /// 
    public void UpdateAOEData(AreaOfEffectData scriptableObject)
    {
        areaOfEffectData = scriptableObject;
        
        AreaOfEffectManager.Instance.AddNewAreaOfEffect(areaOfEffectData);
        
        transform.localScale = areaOfEffectData.aoeSpreadSize * _bonusAoeSize;
        
    }

    public void GiveMiscellaneousModifierList(MiscellaneousModifierList miscellaneousModifierList)
    {
        Debug.Log("Give a bonus for: " + miscellaneousModifierList.BonusAoeRadius);
        _bonusAoeSize = miscellaneousModifierList.BonusAoeRadius;
    }

    public void ResetMiscellaneousBonuses()
    {
        _bonusAoeSize = 1f;
    }
}
