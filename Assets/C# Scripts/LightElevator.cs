using UnityEngine;

public class LightElevator : LightPlatform
{
    public Transform startPoint; // Activated Elevator starts moving from here
    public Transform endPoint; // Activated Elevator stops moving here and starts moving towards startPoint.
    public float speed = 3f; // how fast it moves
    private Transform currentTarget; // which point is the elevator going to
    
    protected override void Start()
    {
        base.Start(); // 1. Run the LightPlatform setup
        currentTarget = endPoint; // 2. Do our specific Elevator setup
    }
    
    void Update()
    {
        if(isActivated)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                SwitchTarget();
            }
        }
    }

    private void SwitchTarget()
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
