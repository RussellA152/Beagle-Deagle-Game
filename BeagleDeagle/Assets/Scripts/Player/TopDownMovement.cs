using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;

public class TopDownMovement : MonoBehaviour, IPlayerDataUpdatable, IMovable
{
    [SerializeField] private CharacterData playerData;
    
    private float _bonusSpeed = 1;

    private bool _canMove;
    public Vector2 MovementInput { get; private set; }
    private Vector2 _rotationInput;
    
    [Header("Required Components")]
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider2D;

    [Header("Weapon Positioning")]
    // Empty object that holds the weapon and player hands
    [SerializeField]  private Transform pivotPoint;
    // How far out should the weapon be from player when rotating (lower values = closer)
    [SerializeField] private float rotationRadius = 0.5f;
    
    
    //[SerializeField]
    //private Animator playerAnimator;
    //[SerializeField]
    //private Animator attackAnimator;
    
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
    }

    ///-///////////////////////////////////////////////////////////
    ///
    void FixedUpdate()
    {

        // Don't allow any movement if the player is not allowed to move (usually on death)
        if (!_canMove) return;
        
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

    ///-///////////////////////////////////////////////////////////
    ///
    //void UpdateMovementAnimation()
    //{
    //    if (movementInput != Vector2.zero)
    //    {
    //        playerAnimator.SetBool("isRunning", true);
    //    }
    //    else
    //    {
    //        playerAnimator.SetBool("isRunning", false);
    //    }

    //}


    #region ControllerCallbacks
    ///-///////////////////////////////////////////////////////////
    ///
    public void OnLook(CallbackContext inputValue)
    {
        if (!_canMove) return;
        
        if (inputValue.ReadValue<Vector2>() != Vector2.zero)
        {
            // If the current input is a mouse
            if (inputValue.control.displayName == "Delta")
            {
                // Find the position of the mouse on the screen
                Vector3 mousePos = Mouse.current.position.ReadValue();

                // Convert that mouse position to a coordinate in world space
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                _rotationInput = worldPos - _capsuleCollider2D.bounds.center;


            }
            // If the current input is a gamepad
            else if (inputValue.control.displayName == "Right Stick")
            {
                // Read _rotationInput straight from the joystick movement
                _rotationInput = inputValue.ReadValue<Vector2>();

            }
        }

    }

    ///-///////////////////////////////////////////////////////////
    /// Get input value when player is moving
    /// 
    public void OnMove(CallbackContext inputValue)
    {
        MovementInput = inputValue.ReadValue<Vector2>();
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

    public void SetMovement(bool boolean)
    {
        _canMove = boolean;
    }

    #endregion


}
