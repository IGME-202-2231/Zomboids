using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : Singleton<FlockManager> { 
    // Start is called before the first frame update

    public List<Agent> flock = new List<Agent>();

    private Vector3 centerPoint;
    private Vector3 sharedDirection;


    public Vector3 SharedDirection
    {
        get { return sharedDirection; }
    }

    public Vector3 CenterPoint { get { return centerPoint; } }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        centerPoint = GetCenterPoint();
        sharedDirection = GetSharedDirection();
    }

    private Vector3 GetCenterPoint()
    {
        Vector3 sumPosition = Vector3.zero;

        foreach (Agent agent in flock)
        {
            sumPosition += agent.transform.position;
        }

        return sumPosition / flock.Count;
    }

    private Vector3 GetSharedDirection()
    {
        Vector3 sumDirection = Vector3.zero;

        foreach(Agent agent in flock)
        {
            sumDirection += agent.transform.up;
        }

        return sumDirection.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(centerPoint, 0.3f);
        Gizmos.DrawLine(centerPoint, sharedDirection + centerPoint);
    }
}
