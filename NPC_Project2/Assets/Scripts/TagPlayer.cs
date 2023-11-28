using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TagStates
{
    NotIt,
    Counting,
    It,
};
public class TagPlayer : Agent
{
    [SerializeField]
    float wanderTime = 1f;

    [SerializeField]
    float wanderRadius = 1f;

    [SerializeField]
    float boundsWeight = 10.0f;

    [SerializeField]
    SpriteRenderer sRenderer;

    //Control Flee Weight
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float fleeWeight = 1;

    TagStates currentState; //Defaults to notit
    float countTimer;

    TagPlayer target;

    protected override void CalcSteeringForces()
    {
        //Switch statement for FSM
        switch (currentState)
        {
            case TagStates.NotIt:
                totalForce += Wander(wanderTime, wanderRadius);
                totalForce += Separate();
                totalForce += Flee(agentManager.itPlayer.transform.position) * fleeWeight;
                break;

            case TagStates.Counting:
                countTimer += Time.deltaTime;
                if(countTimer > agentManager.CountTimer)
                {
                    countTimer = 0;
                    SetState(TagStates.It);
                }
                break;

            case TagStates.It:
                target = FindClosest() as TagPlayer;    //Casts target to TagPlayer
                if(target == null) { break; }

                totalForce += Seek(target.transform.position);

                if(Vector3.Distance(transform.position, target.transform.position) < myPhysicsObject.Radius + target.myPhysicsObject.Radius)
                {
                    SetState(TagStates.NotIt);
                    target.SetState(TagStates.Counting);
                }
                break;
        }
        //Everything stays in bounds all the time
        totalForce += StayInBoundsForce() * boundsWeight;
    }

    public void SetState(TagStates newState)
    {
        currentState = newState;
        sRenderer.sprite = agentManager.tagSprites[(int)currentState];  //Changes sprite state

        //Stops counter from moving
        if(newState == TagStates.Counting)
        {
            myPhysicsObject.StopMoving();
            agentManager.itPlayer = this;    //Tell agentManager that this player is it
        }
    }
}
