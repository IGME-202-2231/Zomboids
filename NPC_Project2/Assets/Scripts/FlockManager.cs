using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : Singleton<FlockManager>
{
    // Start is called before the first frame update
    public List<Agent> flock = new List<Agent>();

    private Vector3 centerPoint;
    private Vector3 sharedDirection;

    public Vector3 SharedDirection
    {
        get { return sharedDirection; }
    }

    public Vector3 CenterPoint { get { return centerPoint; } }

    void Update()
    {
        // Remove null or destroyed agents from the flock
        flock.RemoveAll(agent => agent == null || agent.Equals(null));

        centerPoint = GetCenterPoint();
        sharedDirection = GetSharedDirection();
    }

    private Vector3 GetCenterPoint()
    {
        Vector3 sumPosition = Vector3.zero;
        int validAgentCount = 0;

        foreach (Agent agent in flock)
        {
            if (agent != null && !agent.Equals(null))
            {
                sumPosition += agent.transform.position;
                validAgentCount++;
            }
        }

        if (validAgentCount > 0)
        {
            return sumPosition / validAgentCount;
        }

        // Return a default value if no valid agents are found
        return Vector3.zero;
    }

    private Vector3 GetSharedDirection()
    {
        Vector3 sumDirection = Vector3.zero;
        int validAgentCount = 0;

        foreach (Agent agent in flock)
        {
            if (agent != null && !agent.Equals(null))
            {
                sumDirection += agent.transform.up;
                validAgentCount++;
            }
        }

        if (validAgentCount > 0)
        {
            return sumDirection.normalized;
        }

        // Return a default value if no valid agents are found
        return Vector3.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(centerPoint, 0.3f);
        Gizmos.DrawLine(centerPoint, sharedDirection + centerPoint);
    }

}
