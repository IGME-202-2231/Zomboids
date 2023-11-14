using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleer : Agent
{
    [SerializeField]
    GameObject target;

    Vector3 fleeForce;

    protected override void CalcSteeringForces()
    {
        fleeForce = Flee(target);
        totalForce += fleeForce;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + myPhysicsObject.Velocity);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (fleeForce * 0.3f));
    }
}
