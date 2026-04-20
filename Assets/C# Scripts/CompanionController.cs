using UnityEngine;

public class CompanionController : MonoBehaviour
{
    public Transform flyAround; //A variable that tracks the player and lets the companion fly around it.

    public Transform stickTo; //The companion will be positioned at the firepoint to fire the projectiles.

    public Transform companionHead;

    private PlayerController playerScript; //Stores the player's script.

    public float radius = 3f; //How far the companion can fly around the player.
    public float speed = 4f; // How fast the companion travels.

    public float fireSpeed = 30f; // How fast the companion flies to the firePoint.

    public float idleTime = 3f; // How long before creating a new point to move to.

    public float nextMoveTime; //When the companion will actually start to move again.

    public float lingerDelay = 0.5f; // How long to wait before returning to the idle state.

    private float timeToLeave; // Will store when the companion returns to the idle state.

    public Vector2 currentOffset; // How far the companion needs to be from the player.

    public Vector2 dockOffset; //Fine tune where the companion docks.

    private Vector2 currentLocalPosition; //Handles as if the player is at (0,0)

    private Vector2 lookDirection;

    private bool isFacingRight = true;

    private bool isDocked = false;

    void Start()
    {
        playerScript = flyAround.GetComponent<PlayerController>();
        AssignNewPoint();
    }


    void Update()
    {
        if (playerScript.isFireButtonPressed)
        {
            timeToLeave = Time.time + lingerDelay;
        }

        if(Time.time < timeToLeave)
        {
            
            // 1. Calculate the actual horizontal offset first based on facing direction
            float actualOffsetX;
            if (playerScript.isFacingRight)
            {
                actualOffsetX = dockOffset.x;
            }
            else
            {
                actualOffsetX = -dockOffset.x;
            }

            // 2. Define the exact target position in space by adding the offset to the firepoint
            Vector2 targetDockPosition = (Vector2)stickTo.position + new Vector2(actualOffsetX, dockOffset.y);

            // 3. Measure the distance to THAT specific target spot 
            float distToSlot = Vector2.Distance(transform.position, targetDockPosition);

            if (distToSlot < 0.01f || isDocked) 
            {
                // Snap to the target spot
                transform.position = targetDockPosition; 
                transform.SetParent(flyAround);
                
                // Sync our internal state with the player's state
                isFacingRight = playerScript.isFacingRight;
                isDocked = true;

                // Force the local scale to be positive so it purely inherits the player's flip
                Vector3 fixedScale = transform.localScale;
                fixedScale.x = Mathf.Abs(fixedScale.x); // Mathf.Abs always returns a positive number
                transform.localScale = fixedScale;
            }
            
            else  
            {
                // Fly toward the target spot 
                transform.position = Vector2.MoveTowards(transform.position, targetDockPosition, fireSpeed * Time.deltaTime);

                // If the target is to our right, but we are facing left
                if (targetDockPosition.x > transform.position.x && !isFacingRight)
                {
                    Flip();
                }
                // If the target is to our left, but we are facing right
                else if (targetDockPosition.x < transform.position.x && isFacingRight)
                {
                    Flip();
                }
            }
        

            currentLocalPosition = (Vector2)transform.position - (Vector2)flyAround.position;
        }
        
        else
        {
            //Detach companion from the player
            transform.SetParent(null);
            isDocked = false;
            // Move our local point towards the chosen offset (the flying to point)
            currentLocalPosition = Vector2.MoveTowards(currentLocalPosition, currentOffset, speed * Time.deltaTime);

            if(currentLocalPosition.x > currentOffset.x && isFacingRight)
            {
                Flip();
            }

            else if(currentLocalPosition.x < currentOffset.x && !isFacingRight)
            {
                Flip();
            }

            //Set the companions position in the world
            transform.position = (Vector2)flyAround.position + currentLocalPosition;

            //Assign a new point only after ideling in place for 3 seconds.
            if(Vector2.Distance(currentLocalPosition, currentOffset) > 0.1)
            {
                nextMoveTime = Time.time + idleTime;
            }
            else if(Time.time > nextMoveTime)
            {
                AssignNewPoint();
            }
        }
        
    }

    //Calculates and sets a new point around the player for the companion to fly around.
    public void AssignNewPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle;
        randomPoint.y = Mathf.Abs(randomPoint.y);
        currentOffset = randomPoint * radius;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

    }

    void HeadTilt(Vector2 target)
    {
        lookDirection = target - (Vector2)transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x)*Mathf.Rad2Deg;
        companionHead.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }
}
