using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TopDownMovement : MonoBehaviour, IPlayerDataUpdatable, IMovable
{
    [SerializeField]
    private PlayerEvents playerEvents;
    
    [SerializeField]
    private PlayerData playerData;

    [SerializeField, NonReorderable]
    private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>(); // a list of modifiers being applied to the player's movement speed 

    private float bonusSpeed = 1;

    public Vector2 movementInput { get; private set; }
    Vector2 rotationInput;
    [Header("Required Components")]
    [SerializeField]
    private Rigidbody2D rb;

    //private float moveSpeed;

    [Header("Body Parts")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform head;
    [SerializeField]
    Transform weapon;

    //[SerializeField]
    //private Animator playerAnimator;
    //[SerializeField]
    //private Animator attackAnimator;
    [Header("Sprite Renderers")]
    [SerializeField]
    private SpriteRenderer bodySr;
    [SerializeField]
    private SpriteRenderer tailSr;
    [SerializeField]
    private SpriteRenderer headSr;

    [Range(0, 1)]
    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    ///-///////////////////////////////////////////////////////////
    ///
    //private void Update()
    //{
        //UpdateMovementAnimation();

        //if (weapon.rotation.z >= Quaternion.Euler(0f, 0f, 90f).z || weapon.rotation.z <= Quaternion.Euler(0f,0f,-90f).z)
        //{
        //    Debug.Log("SWAP TO OTHER HAND!");
        //}

    //}

    ///-///////////////////////////////////////////////////////////
    ///
    void FixedUpdate()
    {

        if (movementInput != Vector2.zero)
        {
            //The number of objects we can collide with if we go in this direction
            int count = rb.Cast(movementInput, movementFilter, castCollisions, (playerData.movementSpeed * bonusSpeed) * Time.fixedDeltaTime + collisionOffset);

            //if nothing is in the way, move our character
            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * (playerData.movementSpeed * bonusSpeed) * Time.fixedDeltaTime);
            }

        }

        HandleRotation(rotationInput);
        FlipWithLook(rotationInput);

        //Flip(movementInput, GetComponent<SpriteRenderer>());
        FlipWithMovement(movementInput, bodySr);
        FlipWithMovement(movementInput, tailSr);
    }



    ///-///////////////////////////////////////////////////////////
    ///
    void HandleRotation(Vector2 direction)
    {
        direction.Normalize();

        Vector2 v = direction;

        // adding some random offset to the center
        Vector2 center = (((Vector2)body.position + new Vector2(0f, -3f)) + ((Vector2)head.position + new Vector2(0f,3f))) / 2f;

        v = Vector2.ClampMagnitude(v, 6);

        Vector2 newLocation = center + (v * 1.5f);

        if (direction != Vector2.zero)
        {
            weapon.position = Vector2.Lerp(weapon.position, newLocation, 40 * Time.deltaTime);

            // Rotate towards w/ stick movement
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }


    }


    ///-///////////////////////////////////////////////////////////
    ///
    private void FlipWithMovement(Vector2 input, SpriteRenderer sr)
    {

        if (input.x > 0f)
        {
            sr.flipX = false;
        }
        else if (input.x < 0f)
        {
            sr.flipX = true;
        }
    }

    private void FlipWithLook(Vector2 direction)
    {
        //Debug.Log(direction);

        // if mouse cursor, or joystick is moving to the left of the player,
        // then turn their head to the right
        if (direction.x <= 0f)
            headSr.flipX = false;
        // otherwise, turn their head to the left
        else
            headSr.flipX = true;
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
        //Vector2 playerOffSet = new Vector2()

        if (inputValue.ReadValue<Vector2>() != Vector2.zero)
        {

            // If the current input is a mouse
            if (inputValue.control.displayName == "Delta")
            {
                // Find the position of the mouse on the screen
                Vector3 mousePos = Mouse.current.position.ReadValue();

                // Convert that mouse position to a coordinate in world space
                Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);

                rotationInput = Worldpos - transform.position;


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
    ///
    public void OnMove(CallbackContext inputValue)
    {
        movementInput = inputValue.ReadValue<Vector2>();
    }

    public void UpdateScriptableObject(PlayerData scriptableObject)
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
