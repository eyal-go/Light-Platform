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

    // הפונקציה הזו נקראת אוטומטית כשגוף נכנס לתוך ה-Trigger שלנו
    // זו פונקציה מובנית של יוניטי (Event).
    // היא קופצת אוטומטית ברגע שהקוליידר של הכדור נכנס לתוך קוליידר אחר (שהוא Trigger)
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // בדיקה 1: האם בכלל זיהינו פגיעה במשהו?
        //Debug.Log("Bullet hit object: " + hitInfo.name);

        LightPlatform platform = hitInfo.GetComponent<LightPlatform>();

        if (platform != null)
        {
            // בדיקה 2: האם מצאנו את הסקריפט על האובייקט?
            //Debug.Log("Found LightPlatform script! Activating...");
            
            platform.ActivatePlatform();
            Destroy(gameObject);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        LightPlatform platform = collision.collider.GetComponent<LightPlatform>();

        if(platform != null)
        {
            if(platform is MovingLightPlatform && platform.IsActivated())
            {
                platform.DeactivatePlatform();
                Destroy(gameObject);
            }
        }
    }
}

