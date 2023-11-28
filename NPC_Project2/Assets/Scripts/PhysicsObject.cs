using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    //Physics Fields
    Vector3 position;
    Vector3 velocity;
    Vector3 direction;

    [SerializeField]
    GameObject fleerPrefab;
    [SerializeField]
    bool frictionOn; //Is friction on

    [SerializeField]
    bool gravOn;    //Is Gravity on

    [SerializeField]
    float coeff = 0;    //Coefficient of friction

    [SerializeField]
    float gravStrength;

    [SerializeField]
    Vector3 acceleration;

    [SerializeField]
    float mass = 1;

    [SerializeField]
    float maxSpeed = 1;

    [SerializeField]
    public float radius;

    public float Radius
    {
        get { return radius; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    public Vector3 Velocity
    { 
        get { return velocity; } 
    }

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }    

    public Vector2 ScreenMax
    {
        get { return new Vector2(
            Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect,
            Camera.main.transform.position.y + Camera.main.orthographicSize);
        }
    }

    public Vector2 ScreenMin
    {
        get
        {
            return new Vector2(
            Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect,
            Camera.main.transform.position.y - Camera.main.orthographicSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //********************SAVE FOR LATER**********************************
        if(gravOn)
        {
            ApplyGravity(Vector3.down * 9.81f); //Call Apply Gravity
        }
        

        // Calculate the velocity for this frame - New
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //If frictionOn = true, apply friction
        if(frictionOn)
        {
            ApplyFriction(coeff);
        }

        //BounceOffEdges(); //Call BounceOffEdges to check for collision with edges

        position += velocity * Time.deltaTime;

        // Grab current direction from velocity  - New
        direction = velocity.normalized;

        transform.position = position;

        // Zero out acceleration - New
        acceleration = Vector3.zero;

    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    private void ApplyGravity(Vector3 gravity)
    {
        acceleration += gravity;
    }

    private void BounceOffEdges()
    {
        //Bounce off Sides
        if (position.x <= ScreenMin.x)
        {
            velocity.x *= -1; //Flips x component of the vector
            position.x = ScreenMin.x;
        }
        else if (position.x >= ScreenMax.x)
        {
            velocity.x *= -1;
            position.x = ScreenMax.x;
        }

        //Bounce off Top and Bottom
        if (position.y <= ScreenMin.y)
        {
            velocity.y *= -1; //Flips y component of the vector
            position.y = ScreenMin.y;
        }
        else if (position.y >= ScreenMax.y)
        {
            velocity.y *= -1;
            position.y = ScreenMax.y;
        }

    }

    private void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        ApplyForce(friction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void StopMoving() //Stops counter from moving while applied
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
    }

}
