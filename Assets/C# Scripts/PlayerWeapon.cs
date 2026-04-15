using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint; // fire from
    public GameObject bulletPrefab; // fire what

    public PlayerController playerController; //who fires

    public void Shoot()
    {   /*
        Quaternion bulletRotation;

        if (playerController.isFacingRight)
        {
            bulletRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else 
        {
            bulletRotation = Quaternion.Euler(0f, 0f, 180f);
        }
        */

        // Now we use our custom rotation instead of firePoint.rotation!
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
