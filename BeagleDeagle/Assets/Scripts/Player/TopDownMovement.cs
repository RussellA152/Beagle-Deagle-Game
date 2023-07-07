using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;

public class TopDownMovement : MonoBehaviour, IPlayerDataUpdatable, IMovable
{
    // [SerializeField]
    // private PlayerEvents playerEvents;
    
    [SerializeField]
    private CharacterData playerData;

    [SerializeField, NonReorderable]
    private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>(); // a list of modifiers being applied to the player's movement speed 

    private float bonusSpeed = 1;

    public Vector2 movementInput { get; private set; }
    Vector2 rotationInput;
    
    [Header("Required Components")]
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField] 
    // How far out should the weapon be from player when rotating (lower values = closer)
    private float rotationRadius = 0.5f;

    [SerializeField] 
    // Empty object that holds the weapon and player hands
    private Transform pivotPoint;

    [SerializeField] 
    private CapsuleCollider2D capsuleCollider2D;
    
    //[SerializeField]
    //private Animator playerAnimator;
    //[SerializeField]
    //private Animator attackAnimator;
    
    [Header("Sprite Renderers")]
    [SerializeField]
    private SpriteRenderer bodySr;
    [SerializeField]
    private SpriteRenderer headSr;

    [Range(0, 1)]
    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();
    
    ///-///////////////////////////////////////////////////////////
    ///
    void FixedUpdate()
    {

        if (movementInput != Vector2.zero)
        {
            //The number of objects we can collide with if we go in this direction
            int count = rb.Cast(movementInput, movementFilter, _castCollisions, (playerData.movementSpeed * bonusSpeed) * Time.fixedDeltaTime + collisionOffset);

            //if nothing is in the way, move our character
            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * (playerData.movementSpeed * bonusSpeed) * Time.fixedDeltaTime);
            }

        }

        HandleWeaponRotation(rotationInput);
        
        FlipSpritesWithLook(rotationInput);
        
        FlipSpritesWithMovement(movementInput);
        
    }



    ///-///////////////////////////////////////////////////////////
    ///
    void HandleWeaponRotation(Vector2 direction)
    {
        direction.Normalize();

        Vector2 v = direction * rotationRadius;
        
        v = Vector2.ClampMagnitude(v, 6);

        // Circling around collider, instead of parent transform
        Vector2 newLocation = (Vector2)capsuleCollider2D.bounds.center + v;
        
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

        }
        // Otherwise, turn their head to the right
        else
        {
            headSr.flipX = false;

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
        if (inputValue.ReadValue<Vector2>() != Vector2.zero)
        {
            // If the current input is a mouse
            if (inputValue.control.displayName == "Delta")
            {
                // Find the position of the mouse on the screen
                Vector3 mousePos = Mouse.current.position.ReadValue();

                // Convert that mouse position to a coordinate in world space
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                rotationInput = worldPos - capsuleCollider2D.bounds.center;


            }
            // If the current input is a gamepad
            else if (inputValue.control.displayName == "Right Stick")
            {
                // Read rotationInput straight from the joystick movement
                rotationInput = inputValue.ReadValue<Vector2>();

            }
        }

    }

    ///-///////////////////////////////////////////////////////////
    /// Get input value when player is moving
    /// 
    public void OnMove(CallbackContext inputValue)
    {
        movementInput = inputValue.ReadValue<Vector2>();
    }

    public void UpdateScriptableObject(CharacterData scriptableObject)
    {
        playerData = scriptableObject;
    }

    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd)
    {
        movementSpeedModifiers.Add(modifierToAdd);
        bonusSpeed += bonusSpeed * modifierToAdd.bonusMovementSpeed;

    }

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove)
    {
        movementSpeedModifiers.Remove(modifierToRemove);

        bonusSpeed /= (1 + modifierToRemove.bonusMovementSpeed);

    }
    
    public Vector2 ReturnPlayerDirection()
    {
        return rotationInput;
    }

    #endregion


}
