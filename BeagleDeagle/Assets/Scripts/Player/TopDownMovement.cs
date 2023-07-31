using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TopDownMovement : MonoBehaviour, IPlayerDataUpdatable, IMovable
{
    [SerializeField] private CharacterData playerData;

    private TopDownInput _topDownInput;
    
    private float _bonusSpeed = 1;
    
    private InputAction _movementInputAction;
    private InputAction _rotationInputAction;
    private InputAction _rollInputAction;
    
    public Vector2 MovementInput { get; private set; }
    public bool isRolling { get; private set; }
    
    private Vector2 _rotationInput;
    
    [Header("Required Components")]
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider2D;

    [Header("Weapon Positioning")]
    // Empty object that holds the weapon and player hands
    [SerializeField]  private Transform pivotPoint;
    // How far out should the weapon be from player when rotating (lower values = closer)
    [SerializeField] private float rotationRadius = 0.5f;

    [Header("Sprite Renderers")]
    
    [SerializeField] private SpriteRenderer bodySr;
    
    [SerializeField] private SpriteRenderer headSr;

    
    [SerializeField] private SpriteRenderer weaponSr;

    // [SerializeField] 
    // private SpriteRenderer rightHandSr;
    //
    // [SerializeField] 
    // private SpriteRenderer leftHandSr;
    
    [Header("Modifiers")]
    // A list of modifiers being applied to the player's movement speed
    [SerializeField, NonReorderable] private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>();
    
    [Header("Collisions")]
    [Range(0, 1)]
    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();
    
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        _topDownInput = new TopDownInput();

        _movementInputAction = _topDownInput.Player.Move;
        _rotationInputAction = _topDownInput.Player.Look;
        _rollInputAction = _topDownInput.Player.Roll;


    }

    private void OnEnable()
    {
        _topDownInput.Enable();
        _movementInputAction.performed += OnMove;
        _movementInputAction.canceled += OnMoveCancel;
        _rotationInputAction.performed += OnLook;
        _rollInputAction.performed += OnRoll;
    }

    private void OnDisable()
    {
        _topDownInput.Disable();
        _movementInputAction.performed -= OnMove;
        _movementInputAction.canceled -= OnMoveCancel;
        _rotationInputAction.performed -= OnLook;
        _rollInputAction.performed -= OnRoll;
    }

    private void Start()
    {
        isRolling = false;
    }

    ///-///////////////////////////////////////////////////////////
    ///
    void FixedUpdate()
    {
        
        if (MovementInput != Vector2.zero)
        {
            //The number of objects we can collide with if we go in this direction
            int count = _rb.Cast(MovementInput, movementFilter, _castCollisions, (playerData.movementSpeed * _bonusSpeed) * Time.fixedDeltaTime + collisionOffset);

            //if nothing is in the way, move our character
            if (count == 0)
            {
                _rb.MovePosition(_rb.position + MovementInput * (playerData.movementSpeed * _bonusSpeed) * Time.fixedDeltaTime);
            }

        }

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
        Vector2 newLocation = (Vector2)_capsuleCollider2D.bounds.center + v;
        
        if (direction != Vector2.zero)
        {
            pivotPoint.position = Vector2.Lerp(pivotPoint.position, newLocation, 40 * Time.deltaTime);
        
            // Rotate towards w/ stick movement
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pivotPoint.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }
        
    }


    ///-///////////////////////////////////////////////////////////
    /// Flip the sprite of the player's body depending on which direction
    /// the player is moving in.
    /// 
    private void FlipSpritesWithMovement(Vector2 input)
    {
        if (input.x > 0f)
        {
            bodySr.flipX = false;
            

        }
        else if (input.x < 0f)
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
        if (direction.x <= 0f)
        {
            headSr.flipX = true;
            weaponSr.flipY = true;
        }
        // Otherwise, turn their head to the right
        else
        {
            headSr.flipX = false;
            weaponSr.flipY = false;
        }
    }
    
    public void UpdateScriptableObject(CharacterData scriptableObject)
    {
        playerData = scriptableObject;
    }

    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd)
    {
        movementSpeedModifiers.Add(modifierToAdd);
        _bonusSpeed += _bonusSpeed * modifierToAdd.bonusMovementSpeed;

    }

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove)
    {
        movementSpeedModifiers.Remove(modifierToRemove);

        _bonusSpeed /= (1 + modifierToRemove.bonusMovementSpeed);

    }
    
    public Vector2 ReturnPlayerDirection()
    {
        return _rotationInput;
    }

    #region Inputs

    ///-///////////////////////////////////////////////////////////
    ///
    public void OnMove(CallbackContext context)
    {
        MovementInput = _movementInputAction.ReadValue<Vector2>();
    }

    public void OnMoveCancel(CallbackContext context)
    {
        MovementInput = Vector2.zero;
    }

    public void OnRoll(CallbackContext context)
    {
        Debug.Log("Roll!");
        _rb.isKinematic = false;
        _rb.AddForce(new Vector2(1000f, 0f));

        // Ignore collisions between "Player" and "Enemy" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

        // Ignore collisions between "Player" and "Bullet" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bullet"), true);

        isRolling = true;
    }


    ///-///////////////////////////////////////////////////////////
    ///
    public void OnLook(CallbackContext context)
    {
        if (_rotationInputAction.ReadValue<Vector2>() != Vector2.zero)
        {
            // If the current input is a mouse
            if (context.control.displayName == "Delta")
            {
                // Find the position of the mouse on the screen
                Vector3 mousePos = Mouse.current.position.ReadValue();

                // Convert that mouse position to a coordinate in world space
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                _rotationInput = worldPos - _capsuleCollider2D.bounds.center;


            }
            // If the current input is a gamepad
            else if (context.control.displayName == "Right Stick")
            {
                // Read _rotationInput straight from the joystick movement
                _rotationInput = _rotationInputAction.ReadValue<Vector2>();

            }
        }

    }

    public void AllowMovement(bool boolean)
    {
        if (boolean)
        {
            _movementInputAction.Enable();
            _rotationInputAction.Enable();
        }

        else
        {
            _movementInputAction.Disable();
            _rotationInputAction.Disable();
        }
           
    }

    ///-///////////////////////////////////////////////////////////
    /// At the end of the roll animation, set isRolling to false (* used in roll animation event *)
    /// 
    public void EndRoll()
    {
        // Reset velocity and don't allow player to be affected by forces anymore
        _rb.velocity = Vector2.zero;
        _rb.isKinematic = true;
        
        // Allow collisions between "Player" and "Enemy" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);

        // Allow collisions between "Player" and "Bullet" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bullet"), false);
        
        // Player is no longer rolling at the end of the animation
        isRolling = false;
        
        
    }

    public void AllowRoll(bool boolean)
    {
        if (boolean)
        {
            _rollInputAction.Enable();
        }
        else
        {
            _rollInputAction.Disable();
        }
    }

    #endregion


}
