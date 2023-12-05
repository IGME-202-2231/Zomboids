using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    HumanWander,
    HumanFlee,
    Transformer,
    Zombie,
    Blood
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

    States currentState;
    float countTimer;

    FSM target;


    protected override void CalcSteeringForces()
    {
        //Switch statement for FSM
        switch (currentState)
        {
            case States.HumanWander:
                totalForce += Wander(wanderTime, wanderRadius);
                totalForce += StayInBoundsForce() * boundsWeight;
                totalForce += Separate() * seperateWeight;
                totalForce += Cohesion() * cohesionWight;
                totalForce += Alignment() * alignmentWeight;
                totalForce += AvoidObstacles(avoidTime) * avoidWeight;

                if (agentManager != null && agentManager.ZombieSpawned)
                {
                    SetState(States.HumanFlee);
                }
                break;

            case States.HumanFlee:
                totalForce += Wander(wanderTime, wanderRadius);
                totalForce += Separate();
                totalForce += Flee(agentManager.zombie.transform.position) * fleeWeight;

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
                if (target == null) { break; }

                totalForce += Seek(target.transform.position);

                if (Vector3.Distance(transform.position, target.transform.position) < myPhysicsObject.Radius + target.myPhysicsObject.Radius)
                {
                    target.SetState(States.Transformer);
                }

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
        }
    }
}
