using UnityEngine;

public class MovingLightPlatform : LightPlatform
{
    public Transform startPoint;
    public Transform endPoint;
    private Transform currentTarget;
    public float speed = 3f;

    protected override void Start()
    {
        base.Start();
        currentTarget = endPoint;
    }

    void Update()
    {
        //This platform moves between points while it's deactivated
        if(!isActivated)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed*Time.deltaTime);
            if(Vector2.Distance(transform.position, currentTarget.position) < 0.1) //Switch when reached the currentTarget.
            {
                SwitchTarget();
            }
        }
        
    }

    //Switches the direction the platform is moving to.
    void SwitchTarget()
    {
        if(currentTarget == endPoint)
        {
            currentTarget = startPoint;
        }
        else if(currentTarget == startPoint)
        {
            currentTarget = endPoint;
        }
    }
}
