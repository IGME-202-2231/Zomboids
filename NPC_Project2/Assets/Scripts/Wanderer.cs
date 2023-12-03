using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Agent
{
    [SerializeField]
    float wanderRadius = 1f;

    [SerializeField]
    float wanderTime = 1f;

    [SerializeField]
    [Range(0.0f, 25)]
    float boundsWeight;

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


    protected override void CalcSteeringForces()
    {
        totalForce += Wander(wanderTime, wanderRadius);
        totalForce += StayInBoundsForce() * boundsWeight;
        totalForce += Separate() * seperateWeight;
        totalForce += Cohesion() * cohesionWight;
        totalForce += Alignment() * alignmentWeight;
        totalForce += AvoidObstacles(avoidTime) * avoidWeight;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 futurePosition = CalcFuturePosition(avoidTime);

        float dist = Vector3.Distance(transform.position, futurePosition) + myPhysicsObject.Radius;

        Vector3 boxSize = new Vector3(myPhysicsObject.Radius * 2, dist, myPhysicsObject.Radius * 2);

        Vector3 boxCenter = new Vector3(0, dist/2, 0);

        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        foreach(Vector3 pos in foundObstacles)
        {
            Gizmos.DrawLine(transform.position, pos);
        }

    }
}
