using UnityEngine;

public class LightProjectile : MonoBehaviour
{
    public float speed = 12f;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //here we tell the projectile to change its velocity in accordance to 
        //direction its pointing to.
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, 2f);
    }

    //activates when the prefab attached to the script detects a hit to a GameObject
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        LightPlatform platform = hitInfo.GetComponent<LightPlatform>();

        //If we detected the object we hit is a ghost platform, we activate it.
        if (platform != null)
        {
            platform.ActivatePlatform();
            Destroy(gameObject);
        }
        
    }

    //Deactivte the moving light platform, allowing the player to reposition it.
    //OnCollision here because an activated platform is not a trigger one.
    void OnCollisionEnter2D(Collision2D collision)
    {
        LightPlatform platform = collision.collider.GetComponent<LightPlatform>();
        if(platform != null)
        {
            if(platform is MovingLightPlatform && platform.IsActivated())
            {
                platform.DeactivatePlatform();
            } 
        }
        Destroy(gameObject);
    }
}

