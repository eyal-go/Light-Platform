using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint; // fire from
    public GameObject bulletPrefab; // fire what

    public PlayerController playerController; //who fires

    public void Shoot()
    {   
        // Create the bullet from the fire point and at the rotation calculated in PlayerController.
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
