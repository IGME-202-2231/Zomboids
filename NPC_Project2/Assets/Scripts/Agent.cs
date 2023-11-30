using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour //Cannot instantiate abstract class
{
    [SerializeField]
    protected PhysicsObject myPhysicsObject;

    [SerializeField]
    protected float maxForce = 10;

    protected Vector3 wanderTarget;

    float wanderAngle;
    float perlinOffset;

    protected Vector3 totalForce;

    [SerializeField]
    private float separationRange = 1.0f;

    protected List<Vector3> foundObstacles = new List<Vector3>();

    protected AgentManager agentManager;

    public AgentManager AgentManager
    { set { agentManager = value; } }

    // Start is called before the first frame update
    void Start()
    {
        perlinOffset = Random.Range(0, 1000);
        wanderAngle = Random.Range(0, Mathf.PI * 2);
    }

    // Update is called once per frame
    void Update()
    {
        totalForce = Vector3.zero;

        CalcSteeringForces();

        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
        myPhysicsObject.ApplyForce(totalForce);
    }

    //Must be implemented in an inherited class
    protected abstract void CalcSteeringForces();

    protected Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - transform.position;
        desiredVelocity = desiredVelocity.normalized * myPhysicsObject.MaxSpeed;

        return desiredVelocity - myPhysicsObject.Velocity;
    }

    //Overload Method of Seek
    protected Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    protected Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = transform.position - targetPos;
        desiredVelocity = desiredVelocity.normalized * myPhysicsObject.MaxSpeed;

        return desiredVelocity - myPhysicsObject.Velocity;
    }

    protected Vector3 Flee(GameObject target)
    {
        return Flee(target.transform.position);
    }

    protected Vector3 Wander(float time, float radius)
    {
        Vector3 futurePos = CalcFuturePosition(time);

        //;float randAngle = Random.Range(0.0f, Mathf.PI * 2);
        wanderAngle += (0.5f - (Mathf.PerlinNoise(transform.position.x * 0.1f + perlinOffset, transform.position.y * 0.1f + perlinOffset))) * Mathf.PI * Time.deltaTime;

        Vector3 targetPos = new Vector3(
            Mathf.Cos(wanderAngle) * radius,
            Mathf.Sin(wanderAngle) * radius
        );

        wanderTarget = futurePos + targetPos;

        return Seek(futurePos + targetPos);
    }

    protected Vector3 CalcFuturePosition(float time)
    {
        return transform.position + (myPhysicsObject.Velocity * time);
    }

    protected Vector3 StayInBoundsForce()
    {
        if (transform.position.x < myPhysicsObject.ScreenMin.x ||
           transform.position.x > myPhysicsObject.ScreenMax.x ||
           transform.position.y < myPhysicsObject.ScreenMin.y ||
           transform.position.y > myPhysicsObject.ScreenMax.y)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.z = transform.position.z;
            return Seek(cameraPosition);
        }
        return Vector3.zero;
    }

    protected Vector3 Separate()
    {
        Vector3 separateForce = Vector3.zero;

        foreach (Agent a in agentManager.Agents)
        {
            if (a == this) { continue; }

            float distance = Vector3.Distance(transform.position, a.transform.position);
            distance += 0.000001f;

            separateForce += Flee(a.transform.position) * (separationRange / distance);
        }

        return separateForce;
    }

    //Finds closest agent by starting with null and returning nearest.
    protected Agent FindClosest()
    {
        float minDist = Mathf.Infinity;
        Agent nearest = null;

        foreach (Agent a in agentManager.Agents)
        {
            if (a == this) { continue; }

            float dist = Vector2.Distance(transform.position, a.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = a;
            }
        }

        return nearest;
    }

    protected Vector3 Cohesion()
    {
        return Seek(FlockManager.Instance.CenterPoint);
    }

    protected Vector3 Alignment()
    {
        Vector3 desiredVelocity = FlockManager.Instance.SharedDirection *
            myPhysicsObject.MaxSpeed;

        return desiredVelocity - myPhysicsObject.Velocity;
    }

    protected Vector3 AvoidObstacles(float avoidTime)
    {
        Vector3 avoidForce = Vector3.zero;
        foundObstacles.Clear();

        Vector3 futurePosition = CalcFuturePosition(avoidTime);
        float maxDistance = Vector3.Distance(transform.position, futurePosition) + myPhysicsObject.Radius;

        //Detect and avoid obstacles
        foreach(Obstacle obst in agentManager.obstacles)
        {
            Vector3 agentToObstacle = obst.transform.position - transform.position;
            float forwardDot = Vector3.Dot(agentToObstacle, myPhysicsObject.Velocity.normalized);

            Vector3 rightward = Vector3.Cross(myPhysicsObject.Velocity.normalized, Vector3.forward);
            float rightDot = Vector3.Dot(agentToObstacle, rightward);

            if (forwardDot >= -obst.radius &&
                forwardDot <= (maxDistance + obst.radius) &&
                Mathf.Abs(rightDot) <= (myPhysicsObject.Radius + obst.radius))
            {
                //Refine this to only be obstacles in the way
                foundObstacles.Add(obst.transform.position);

                if(rightDot > 0) //if obstacle is to the right
                {
                    //Go Left
                    avoidForce += transform.right * -1;

                }
                else //if obstacle is to the left or in front
                {
                    //Go Right
                    avoidForce += transform.right;
                }
            }
        }

        return avoidForce;
    }
}
