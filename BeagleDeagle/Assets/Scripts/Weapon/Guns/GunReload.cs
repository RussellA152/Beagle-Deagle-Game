using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

public class GunReload : MonoBehaviour, IRegisterModifierMethods,IHasCooldown, IHasInput, IGunDataUpdatable
{
    [Header("Required Components")]
    [SerializeField] private PlayerEvents playerEvents;
    private CooldownSystem _cooldownSystem;
    private ModifierManager _modifierManager;
    private AudioClipPlayer _audioClipPlayer;
    private GunShooting _gunShooting;
    
    private GunData _weaponData;
    
    private PlayerInput _playerInput;
    
    private InputAction _reloadInputAction;
    
    public bool IsReloading { get; private set; }
    private bool _canReload;
    
    private int _bulletsShot; // How much ammo has the player shot since the last reload or refill?
    
    private int _bulletsLoaded; // How much ammo is currently in the magazine?
    
    [SerializeField, NonReorderable]
    private List<ReloadSpeedModifier> reloadSpeedModifiers = new List<ReloadSpeedModifier>();
    [SerializeField, NonReorderable]
    private List<AmmoLoadModifier> ammoLoadModifiers = new List<AmmoLoadModifier>();
    
    private float _bonusReloadSpeed = 1f;
    private float _bonusAmmoLoad = 1f;

    private void Awake()
    {
        // PlayerInput component is located in parent gameObject (the Player)
        _playerInput = GetComponentInParent<PlayerInput>();
        
        _audioClipPlayer = GetComponentInParent<AudioClipPlayer>();
        
        // Fetch cooldown system component from the player gameObject (always the parent of their gun)
        _cooldownSystem = GetComponentInParent<CooldownSystem>();
        
        _modifierManager = GetComponentInParent<ModifierManager>();

        _gunShooting = GetComponent<GunShooting>();
        
        _reloadInputAction = _playerInput.currentActionMap.FindAction("Reload");
        
        _reloadInputAction.performed += OnReload;
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();
    }

    private void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
        
        UpdateScriptableObject(_weaponData);
        
        _bulletsLoaded = Mathf.RoundToInt(_bulletsLoaded * _bonusAmmoLoad);
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        
        playerEvents.InvokeReloadCooldown(Id);
        
        _bulletsShot = 0;
        
    }
    
    private void OnEnable()
    {
        _cooldownSystem.OnCooldownEnded += OnReloadFinish;

    }
    
    private void OnDisable()
    {
        _cooldownSystem.OnCooldownEnded -= OnReloadFinish;
        _reloadInputAction.Disable();
        
        _reloadInputAction.performed -= OnReload;
    }

    private void Update()
    {
        /* If the player has no ammo loaded into their weapon, begin reloading
         If the gun is not already reloading, begin a coroutine for the reload */
        if (_bulletsLoaded <= 0f && !_cooldownSystem.IsOnCooldown(Id) && _reloadInputAction.enabled)
        {
            PerformReload();
            return;
        }
    }
    
    private void PerformReload()
    {
        if (!_cooldownSystem.IsOnCooldown(Id) && _canReload)
        {
            IsReloading = true;
            
            //_shootInput = 0f;
            _cooldownSystem.PutOnCooldown(this);
            
            // Play "reloadStart" sound effect
            _audioClipPlayer.PlayGeneralAudioClip(_weaponData.gunEffectsData.reloadStartClip,
                _weaponData.gunEffectsData.reloadSoundVolume);
            
            // Start playing the reload finished sound effect
            StartCoroutine(PlayReloadFinishedSound());
            
            // ActuallyShooting = false;
            // _gunShooting.AllowShoot(false;
        }
    }
    
    // Call reload function when the player presses the reload key
    public void OnReload(CallbackContext context)
    {
        // Only allow manual reloading if the player has some ammo, but not empty
        if(_bulletsLoaded > 0f && _bulletsShot > 0 && _bulletsLoaded < _weaponData.magazineSize * _bonusAmmoLoad)
            PerformReload();
    }

    private void OnReloadFinish(int id)
    {
        if (id != Id) return;

        RefillAmmoCompletely();
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);

        _gunShooting.AllowShoot(true);
        
        playerEvents.InvokePlayerFinishedReloadEvent();
        
        IsReloading = false;

    }

    public bool CheckAmmo()
    {
        // If player has no ammo in reserve,
        // then force them to reload by returning false
        return _bulletsLoaded > 0f;
    }

    public void UpdateAmmo()
    {
        _bulletsShot++;
            
        _bulletsLoaded--;
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
    }
    
    ///-///////////////////////////////////////////////////////////
    /// When the gun is done reloading, refill all the ammo
    /// 
    private void RefillAmmoCompletely()
    {
        _bulletsLoaded = Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad);
        _bulletsShot = 0;
    }
    
    public void AllowReload(bool boolean)
    {
        _canReload = boolean;
    }
    
    private IEnumerator PlayReloadFinishedSound()
    {
        float halfDuration = CooldownDuration / 2f;
        float eightyPercentDuration = CooldownDuration * 0.8f;

        // Wait for 50% of the cooldown duration
        yield return new WaitForSeconds(halfDuration);
    
        // Play "reloadFinished" sound effect at 50% completion
        _audioClipPlayer.PlayGeneralAudioClip(_weaponData.gunEffectsData.reloadFinishedClip, _weaponData.gunEffectsData.reloadSoundVolume);

        // If the gun is not empty upon reloading, then don't play a reload slide sound
        if (_bulletsLoaded != 0)
            yield break;

        // Calculate the remaining time to reach 80% completion
        float remainingTime = eightyPercentDuration - halfDuration;
    
        // Wait for the remaining time
        yield return new WaitForSeconds(remainingTime);

        // Play "reloadSlide" sound effect at 80% completion
        _audioClipPlayer.PlayGeneralAudioClip(_weaponData.gunEffectsData.reloadSlideClip, _weaponData.gunEffectsData.reloadSoundVolume);
    }
    
    public void AddReloadSpeedModifier(ReloadSpeedModifier modifierToAdd)
    {
        if (reloadSpeedModifiers.Contains(modifierToAdd)) return;
        
        reloadSpeedModifiers.Add(modifierToAdd);
        _bonusReloadSpeed += _bonusReloadSpeed * modifierToAdd.bonusReloadSpeed;
        
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
    }

    public void RemoveReloadSpeedModifier(ReloadSpeedModifier modifierToRemove)
    {
        if (!reloadSpeedModifiers.Contains(modifierToRemove)) return;
        
        reloadSpeedModifiers.Remove(modifierToRemove);
        _bonusReloadSpeed /= (1 + modifierToRemove.bonusReloadSpeed);
        
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
        
    }

    public void AddAmmoLoadModifier(AmmoLoadModifier modifierToAdd)
    {
        if (ammoLoadModifiers.Contains(modifierToAdd)) return;
        
        ammoLoadModifiers.Add(modifierToAdd);
        _bonusAmmoLoad += _bonusAmmoLoad * modifierToAdd.bonusAmmoLoad;
        
        // Give player's weapon this bonus ammo load (this is because bulletsLoaded is only inside of the SO)
        // Refill the player's weapon before applying new ammo load
        RefillAmmoCompletely();

        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        playerEvents.InvokeUpdateMaxAmmoLoadedText(Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad));
        
    }

    public void RemoveAmmoLoadModifier(AmmoLoadModifier modifierToRemove)
    {
        if (!ammoLoadModifiers.Contains(modifierToRemove)) return;
        
        ammoLoadModifiers.Remove(modifierToRemove);
        _bonusAmmoLoad /= (1 + modifierToRemove.bonusAmmoLoad);
        
        _bulletsLoaded = Mathf.RoundToInt(_bulletsLoaded * _bonusAmmoLoad);
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        playerEvents.InvokeUpdateMaxAmmoLoadedText(Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad));
    }
    
    public void AllowInput(bool boolean)
    {
        if (boolean)
        {
            _reloadInputAction.Enable();
        }
        else
        {
            _reloadInputAction.Disable();
        }
    }
    
    public void UpdateScriptableObject(GunData scriptableObject)
    {
        _weaponData = scriptableObject;
        
        // Refill all ammo after receiving new weapon
        RefillAmmoCompletely();
        
        // Stop reloading if player switched to a new gun (ammo will refill anyways)
        _cooldownSystem.EndCooldown(Id);
        
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        playerEvents.InvokeUpdateMaxAmmoLoadedText(Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad));
    }

    public GunData GetCurrentData()
    {
        return _weaponData;
    }
    
    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<ReloadSpeedModifier>(AddReloadSpeedModifier);
        _modifierManager.RegisterAddMethod<AmmoLoadModifier>(AddAmmoLoadModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<ReloadSpeedModifier>(RemoveReloadSpeedModifier);
        _modifierManager.RegisterRemoveMethod<AmmoLoadModifier>(RemoveAmmoLoadModifier);
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }

}
