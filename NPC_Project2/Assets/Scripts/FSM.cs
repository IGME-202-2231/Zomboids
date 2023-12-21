using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    HumanWander,
    HumanFlee,
    Transformer,
    Zombie,
    Blood,
    Truck,
    Corgi
};
public class FSM : Agent
{
    [SerializeField]
    float wanderTime = 1f;

    [SerializeField]
    float wanderRadius = 1f;

    [SerializeField]
    float boundsWeight = 10.0f;

    [SerializeField]
    SpriteRenderer sRenderer;

    [SerializeField]
    [Range(0.0f, 25f)]
    float wanderWeight;

    [SerializeField]
    [Range(0.0f, 25)]
    float seperateWeight;

    [SerializeField]
    [Range(0.0f, 25f)]
    float cohesionWight;

    [SerializeField]
    [Range(0.0f, 25f)]
    float alignmentWeight;

    [SerializeField]
    [Range(0.0f, 25f)]
    float avoidWeight;

    [SerializeField]
    float avoidTime = 1.0f;

    //Control Flee Weight
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float fleeWeight = 1;

    public States currentState;
    float countTimer;

    FSM target;

    public Splatter splatter;

    private void Start()
    {
        splatter = FindObjectOfType<Splatter>();
    }
    protected override void CalcSteeringForces()
    {
        // Check if the agentManager is null
        if (agentManager == null)
        {
            // Handle the case where agentManager is null (perhaps log a message or take appropriate action)
            return;
        }

        //Switch statement for FSM
        switch (currentState)
        {
            case States.HumanWander:
                totalForce += Wander(wanderTime, wanderRadius);
                totalForce += Separate() * seperateWeight;
                totalForce += Cohesion() * cohesionWight;
                totalForce += Alignment() * alignmentWeight;
                totalForce += AvoidObstacles(avoidTime) * avoidWeight;

                // Check if agentManager and ZombieSpawned are not null before accessing them
                if (agentManager.ZombieSpawned)
                {
                    SetState(States.HumanFlee);
                }
                break;

            case States.HumanFlee:
                totalForce += Flee(agentManager.zombie.transform.position) * fleeWeight;
                totalForce += Wander(wanderTime, wanderRadius);
                totalForce += Separate() * seperateWeight;
                totalForce += Cohesion() * cohesionWight;
                totalForce += Alignment() * alignmentWeight;
                totalForce += AvoidObstacles(avoidTime) * avoidWeight;

                break;

            case States.Transformer:
                countTimer += Time.deltaTime;
                if (countTimer > agentManager.CountTimer)
                {
                    countTimer = 0;
                    SetState(States.Zombie);
                }
                break;

            case States.Zombie:
                target = FindClosest() as FSM;
                // Check if the target is null before accessing it
                if (target != null)
                {
                    // Check if the target is a HumanFlee before applying Seek force
                    if (target.currentState == States.HumanFlee)
                    {
                        totalForce += Seek(target.transform.position);

                        // Check for collision using Box Colliders
                        if (CheckCollision(target.gameObject))
                        {
                            target.SetState(States.Transformer);
                        }
                    }
                }
                break;

            case States.Corgi:
                target = FindClosest() as FSM;
                // Check if the target is null before accessing it
                if (target != null)
                {
                    // Check if the target is a HumanFlee before applying Seek force
                    if (target.currentState == States.Zombie || target.currentState == States.Transformer)
                    {
                        totalForce += Seek(target.transform.position);

                        // Check for collision using Box Colliders
                        if (CheckCollision(target.gameObject))
                        {
                            target.SetState(States.Blood);
                        }
                    }
                }
                break;

            case States.Blood:
                myPhysicsObject.StopMoving();
                break;
        }

        //Everything stays in bounds all the time
        totalForce += StayInBoundsForce() * boundsWeight;
    }

    public void SetState(States newState)
    {
        currentState = newState;
        sRenderer.sprite = agentManager.tagSprites[(int)currentState];  //Changes sprite state

        //Stops counter from moving
        if (newState == States.Transformer)
        {
            myPhysicsObject.StopMoving();
            agentManager.zombie = this;    //Tell agentManager that this player is a zombie
            splatter.PlaySplatSound();
        }

        if (newState == States.Blood)
        {
            // Disable the Animator component
            if (myPhysicsObject.animator != null)
            {
                myPhysicsObject.animator.enabled = false;
            }
            splatter.PlaySplatSound();
        }

        //if (newState == States.Corgi)
        //{
        //    splatter.PlaySplatSound();
        //}
    }

    private bool CheckCollision(GameObject otherObject)
    {
        // Check if the Box Colliders of the two GameObjects are overlapping
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        BoxCollider2D otherCollider = otherObject.GetComponent<BoxCollider2D>();

        if (myCollider != null && otherCollider != null)
        {
            return myCollider.bounds.Intersects(otherCollider.bounds);
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 futurePosition = CalcFuturePosition(avoidTime);

        float dist = Vector3.Distance(transform.position, futurePosition) + myPhysicsObject.Radius;

        Vector3 boxSize = new Vector3(myPhysicsObject.Radius * 2, dist, myPhysicsObject.Radius * 2);

        Vector3 boxCenter = new Vector3(0, dist / 2, 0);

        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        foreach (Vector3 pos in foundObstacles)
        {
            Gizmos.DrawLine(transform.position, pos);
        }

    }
}
