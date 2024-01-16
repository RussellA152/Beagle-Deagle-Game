using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;

public class TopDownMovement : MonoBehaviour, IPlayerDataUpdatable, IMovable, IHasCooldown, IHasInput, IRegisterModifierMethods
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerEvents playerEvents;
    
    private CooldownSystem _cooldownSystem;
    private AudioClipPlayer _audioClipPlayer;
    private ModifierManager _modifierManager;
    private ModifierParticleEffectHandler _modifierParticleEffectHandler;

    private PlayerInput _playerInput;

    private float _bonusSpeed = 1f;
    
    // Input actions needed for movement
    private InputAction _movementInputAction;
    private InputAction _rotationInputAction;
    private InputAction _rollInputAction;
    
    // Input from user for movement and rotation
    public Vector2 MovementInput { get; private set; }
    private bool _canMove = true;
    private Vector2 _rotationInput;
    private bool _canRotate = true;
    public bool IsRolling { get; private set; }
    private bool _canRoll = true;
    
    
    
    [Header("Required Components")]
    private Rigidbody2D _rb;
    [SerializeField] private CapsuleCollider2D aimCollider;
    
    [Header("Weapon Positioning")]
    // Empty object that holds the weapon and player hands
    [SerializeField]  private Transform weaponAimTransform;
    // How far out should the weapon be from player when rotating (lower values = closer)
    [Range(0.1f, 0.5f)]
    [SerializeField] private float rotationRadius = 0.1f;

    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer bodySr;
    [SerializeField] private SpriteRenderer headSr;


    [Header("Modifiers")]
    // A list of modifiers being applied to the player's movement speed
    [SerializeField, NonReorderable] private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _playerInput = GetComponent<PlayerInput>();

        _audioClipPlayer = GetComponent<AudioClipPlayer>();
        _modifierManager = GetComponent<ModifierManager>();

        // Set up input for user to control movement
        _movementInputAction = _playerInput.currentActionMap.FindAction("Move");
        _rotationInputAction = _playerInput.currentActionMap.FindAction("Look");
        _rollInputAction = _playerInput.currentActionMap.FindAction("Roll");

        _cooldownSystem = GetComponent<CooldownSystem>();

        _modifierParticleEffectHandler = GetComponent<ModifierParticleEffectHandler>();
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();

    }

    private void OnEnable()
    {
        _movementInputAction.performed += OnMove;
        _movementInputAction.canceled += OnMoveCancel;
        _rotationInputAction.performed += OnLook;
        _rollInputAction.performed += OnRoll;
    }

    private void OnDisable()
    {
        _movementInputAction.performed -= OnMove;
        _movementInputAction.canceled -= OnMoveCancel;
        _rotationInputAction.performed -= OnLook;
        _rollInputAction.performed -= OnRoll;
    }

    private void Start()
    {
        IsRolling = false;
        
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = playerData.rollCooldown;
        
        playerEvents.InvokeRollCooldown(Id);
    }

    ///-///////////////////////////////////////////////////////////
    ///
    void FixedUpdate()
    {
        if (MovementInput != Vector2.zero && !IsRolling)
        {
            _rb.MovePosition(_rb.position + MovementInput * (playerData.movementSpeed * _bonusSpeed) * Time.fixedDeltaTime);
        }
        
        if(_canRotate)
            HandleWeaponRotation(_rotationInput);
        
        FlipSpritesWithLook(_rotationInput);
        
        FlipSpritesWithMovement(MovementInput);
        
    }



    ///-///////////////////////////////////////////////////////////
    ///
    void HandleWeaponRotation(Vector2 direction)
    {
        direction.Normalize();

        Vector2 v = direction * rotationRadius;
        
        v = Vector2.ClampMagnitude(v, 6);

        // Circling around collider, instead of parent transform
        Vector2 newLocation = (Vector2)aimCollider.bounds.center + v;
        
        if (direction != Vector2.zero)
        {
            weaponAimTransform.position = Vector2.Lerp(weaponAimTransform.position, newLocation, 40 * Time.deltaTime);
        
            // Rotate towards w/ stick movement
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponAimTransform.rotation = Quaternion.Euler(0f, 0f, zRotation);

            Vector3 localScale = Vector3.one;

            if (zRotation > 90 || zRotation < -90)
            {
                localScale.y = -1f;
            }
            else
            {
                localScale.y = +1f;
            }

            weaponAimTransform.localScale = localScale;
        }
        
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Flip the sprite of the player's body depending on which direction
    /// the player is moving in.
    /// 
    private void FlipSpritesWithMovement(Vector2 input)
    {
        if (input.x > 0f && !IsRolling)
        {
            bodySr.flipX = false;
            

        }
        else if (input.x < 0f && !IsRolling)
        {
            bodySr.flipX = true;


        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Flip the sprite of the player's head and hands depending on which
    /// direction the player is looking at (with mouse or gamepad stick).
    /// 
    private void FlipSpritesWithLook(Vector2 direction)
    {
        // If mouse cursor, or joystick is moving to the left of the player,
        // then turn their head to the left
        if (direction.x <= 0f && !IsRolling)
        {
            headSr.flipX = true;
        }
        // Otherwise, turn their head to the right
        else if(!IsRolling)
        {
            headSr.flipX = false;
        }
    }
    
    public void UpdateScriptableObject(PlayerData scriptableObject)
    {
        playerData = scriptableObject;
    }

    #region MovementModifiers
    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd)
    {
        if (movementSpeedModifiers.Contains(modifierToAdd)) return;
        
        movementSpeedModifiers.Add(modifierToAdd);
        _bonusSpeed += _bonusSpeed * modifierToAdd.bonusMovementSpeed;

        _modifierParticleEffectHandler.StartPlayingParticle(modifierToAdd, true);
    }

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove)
    {
        if (!movementSpeedModifiers.Contains(modifierToRemove)) return;
        
        _modifierParticleEffectHandler.StopSpecificParticle(modifierToRemove);
        
        movementSpeedModifiers.Remove(modifierToRemove);

        _bonusSpeed /= (1 + modifierToRemove.bonusMovementSpeed);

    }
    

    #endregion
    
    
    public Vector2 ReturnPlayerDirection()
    {
        return _rotationInput;
    }

    #region Inputs

    ///-///////////////////////////////////////////////////////////
    ///
    private void OnMove(CallbackContext context)
    {
        if(_canMove)
            MovementInput = _movementInputAction.ReadValue<Vector2>();
    }

    private void OnMoveCancel(CallbackContext context)
    {
        _rb.velocity = Vector2.zero;
        MovementInput = Vector2.zero;
    }

    public void OnRoll(CallbackContext context)
    {
        if (!_cooldownSystem.IsOnCooldown(Id) && _canRoll)
        {
            // Put roll on cooldown
            _cooldownSystem.PutOnCooldown(this);
            
            _rb.velocity = Vector2.zero;

            // Add force in the direction the player is moving in, otherwise just roll in the right direction
            if(MovementInput != Vector2.zero)
                _rb.AddForce(new Vector2(playerData.rollPower.x * MovementInput.x, playerData.rollPower.y * MovementInput.y));
            else
                _rb.AddForce(new Vector2(playerData.rollPower.x, 0f));
            
            RollDirection();
            
            _audioClipPlayer.PlayRandomGeneralAudioClip(playerData.rollSounds, playerData.rollSoundVolume);

            // Ignore collisions between "Player" and "HitBox" layers
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("HitBox"), true);
        
            // Ignore collisions between "Player" and "Bullet" layers
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bullet"), true);

            IsRolling = true;
        }
    }

    private void RollDirection()
    {
        Vector2 originalWeaponScale = weaponAimTransform.localScale;
        if (MovementInput.x >= 0f)
        {
            headSr.flipX = false;
            bodySr.flipX = false;
            weaponAimTransform.localScale = new Vector3(1f, 1f, 1f);

        }
        else
        {
            headSr.flipX = true;
            bodySr.flipX = true;
            weaponAimTransform.localScale = new Vector3(1f, -1f, 1f);
            
        }
        
        StartCoroutine(RotateGun(originalWeaponScale));
    }

    ///-///////////////////////////////////////////////////////////
    /// While the player is rolling, rotate their gun 360 degrees
    /// 
    IEnumerator RotateGun(Vector2 originalScale)
    {
        float endZRot;
        
        if (MovementInput.x >= 0f)
            // Negative value for clockwise rotation
            endZRot = -360f;
            
        else
            // Positive value for counter-clockwise rotation
            endZRot = 360f;

        Quaternion startRotation = weaponAimTransform.transform.rotation;
        
        // The time it takes to complete the rotation in seconds
        // TODO: Make roll duration?
        float duration = .5f;
        
        float t = 0;

        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / duration);
            Vector3 newEulerOffset = Vector3.forward * (endZRot * t);
            // global z rotation
            weaponAimTransform.transform.rotation = Quaternion.Euler(newEulerOffset) * startRotation;
            yield return null;
        }

        // Ensure the final rotation is exactly what you expect
        //Vector3 finalEulerOffset = Vector3.forward * endZRot;
        //weaponAimTransform.transform.rotation = Quaternion.Euler(finalEulerOffset) * startRotation;
        
        float zRotation = Mathf.Atan2(_rotationInput.y, _rotationInput.x) * Mathf.Rad2Deg;
        weaponAimTransform.rotation = Quaternion.Euler(0f, 0f, zRotation);

        weaponAimTransform.localScale = originalScale;
    }

    ///-///////////////////////////////////////////////////////////
    /// At the end of the roll animation, set isRolling to false (* used in roll animation event *)
    /// 
    public void EndRoll()
    {

        // Reset velocity and don't allow player to be affected by forces anymore
        _rb.velocity = Vector2.zero;

        // Allow collisions between "Player" and "HitBox" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("HitBox"), false);
        // Allow collisions between "Player" and "Bullet" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bullet"), false);
        
        // Player is no longer rolling at the end of the animation
        IsRolling = false;
        
        
    }
    
    ///-///////////////////////////////////////////////////////////
    ///
    public void OnLook(CallbackContext context)
    {
        if (_rotationInputAction.ReadValue<Vector2>() != Vector2.zero)
        {
            switch (context.control.displayName)
            {
                // If the current input is a mouse
                case "Delta":
                    // Find the position of the mouse on the screen
                    Vector3 mousePos = Mouse.current.position.ReadValue();

                    // Convert that mouse position to a coordinate in world space
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                    _rotationInput = worldPos - aimCollider.bounds.center;
                    break;
                
                // If the current input is a gamepad
                case "Right Stick":
                    // Read _rotationInput straight from the joystick movement
                    _rotationInput = _rotationInputAction.ReadValue<Vector2>();
                    break;
            }
        }
    }

    public void AllowMovement(bool boolean)
    {
        _canMove = boolean;

    }

    public void AllowRotation(bool boolean)
    {
        _canRotate = boolean;
    }

    public void AllowRoll(bool boolean)
    {
        _canRoll = boolean;
    }

    #endregion
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
    
    public void AllowInput(bool boolean)
    {
        if (boolean)
        {
            _movementInputAction.Enable();
            _rotationInputAction.Enable();
            _rollInputAction.Enable();
        }
        
        else
        {
            _movementInputAction.Disable();
            _rotationInputAction.Disable();
            _rollInputAction.Disable();
        }
    }

    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<MovementSpeedModifier>(AddMovementSpeedModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<MovementSpeedModifier>(RemoveMovementSpeedModifier);
    }
}
