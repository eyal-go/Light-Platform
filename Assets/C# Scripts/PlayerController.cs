using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    //variables for the player's movement and raygun.
    public float speed = 5f;
    public float jumpForce = 10f;
    //Rigidbody is Unity's way to handle physics
    private Rigidbody2D rb;
    public bool isGrounded = true;
    //using layers to distinguish between what we want to interact with and what not
    public LayerMask groundLayer;
    public bool isFacingRight = true; //Boolean var to track which direction the player is facing; right by default
    public Transform firePoint; //Adjecent to the player where the bullets fly from
    
    private Vector2 movementInput; //Stores coordinates for the direction the player is pressing

    private Vector2 pointerPosition; //Stores mouse/controller location

    private Vector2 lookDirection;

    public PlayerWeapon weapon;

    public bool isFireButtonPressed; 


    //function that runs 1 time, sort of a constructor.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //function that creates the motion: this function runs ~60 times per second.
    void Update()
    {
        //Get the direction that the player is pressing
        float moveX = movementInput.x;

        //Here we get a variable to calculate relative position of the mouse and the player
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(pointerPosition);
        lookDirection = mousePosition - transform.position;

        //Get the angle of the mouse relative to the player
        float lookAngle = Mathf.Rad2Deg*Mathf.Atan2(lookDirection.y, lookDirection.x);
        

        if(isFireButtonPressed)
        {
            // 1. Handle player facing direction
            if(lookDirection.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if(lookDirection.x < 0 && isFacingRight)
            {
                Flip();
            }
        }

        //Making sure the player faces the direction pressed.
        else if(moveX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if(moveX < 0 && isFacingRight)
        {
            Flip();
        }




        //set the weapon to face that direction
        firePoint.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        // 1. We only cast the ray if the player is actively pressing left or right
        if (moveX != 0)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1f, 0.9f), 0f, new Vector2(moveX, 0), 0.05f, groundLayer);

            // 2. Check for the wall
            if (hit.collider == null)
            {
                // Path is clear, move normally
                rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
            }
            else
            {
                // Hit a wall, stop horizontal movement
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }
        else
        {
            // 3. Stop horizontal movement when the player releases the keys
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }


        //handle grounding logic

        RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, new Vector2(1f, 0.1f), 0f, Vector2.down, 0.6f, groundLayer);
        if(groundHit.collider == null)
        {
            if(isGrounded)
                isGrounded = false;
        }
        else if(!isGrounded)
        {
            isGrounded = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        //Attach player to elevator if it steps on it
        LightElevator elevator = collision.gameObject.GetComponent<LightElevator>();
        if(elevator != null)
        {
            transform.SetParent(collision.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
    
        //Check if player left an elevator, and detach it if it did
        LightElevator elevator = collision.gameObject.GetComponent<LightElevator>();
        if(elevator != null && gameObject.activeInHierarchy)
        {
            transform.SetParent(null);
        }
    }
        
    //Flips the direction relative to the X axis of the transform of the player
    void Flip()
    {
        // 1. Toggle our tracking boolean
        isFacingRight = !isFacingRight;

        // 2. Mirror the player's body and eye using the Reflection matrix (Scale)
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    //Using Input System Package to handle inputs
    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if(isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    public void OnLook(InputValue value)
    {
        pointerPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {

        isFireButtonPressed = value.isPressed;

        //shoot only when button is released.
        if(!isFireButtonPressed)
        {
            weapon.Shoot();
        }           
    }
}
