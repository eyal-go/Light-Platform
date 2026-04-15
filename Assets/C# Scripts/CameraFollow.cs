using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Transform handles rotation, scale and position of the GameObject it is attached to
    public Transform target;

    //Vector3 here to control depth of the camera
    public Vector3 offset;

    //LateUpdate is called after all Update functions, to ensure smooth follow with collisions between what was
    //calculated first.
    void LateUpdate()
    {
        /*Update the camera's position and offset. Since we extend MonoBehaviour, we can 
        edit the properties of the offset in the inspector window in the Editor.
        */
        transform.position = target.position + offset;
    }
}
