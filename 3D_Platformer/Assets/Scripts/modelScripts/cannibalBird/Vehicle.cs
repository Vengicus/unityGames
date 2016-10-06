using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

public abstract class Vehicle : MonoBehaviour 
{

    //Public fields
    public float maxSpeed = 2.5f;
    public float maxForce = 3.0f;
    public float mass = 1.0f;
    public float radius = 1.0f;
    public float gravity = 20.0f;

    protected CharacterController charController;

    //Necessary vectors for movement, inspired by 2D movement in processing
    protected Vector3 acceleration;         //Change in velocity per second
    protected Vector3 velocity;             //Change in position per second
    protected Vector3 desiredVelocity;      //Helps steer the vehicle in the right direction

    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }





	// Use this for initialization
	public virtual void Start () 
	{
        //Gets an internal reference to the character controller script that is on THIS vehicle
        charController = this.gameObject.GetComponent<CharacterController>();


        acceleration = Vector3.zero;        //Initialized as a 0,0,0 vector, neat shortcut, no need for new Vector3

        velocity = transform.forward;       //Unity KEEPS TRACK of its forward vector and it is already pre-normalized
	}
	
	// Update is called once per frame
	protected void Update () 
	{
        //Calculate Steering Forces to apply to the vehicle and give a vector to move in the right direction
        CalcSteeringForces();

        //Update the velocity on the vehicle
        velocity += acceleration * Time.deltaTime;  //Smooth movement independent of framerate
        velocity.y = 0;                             //Keep us moving in 2D or the X,Z plane
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
                         //ClampMagnitude(Vector to be limited, value to be limited by)

        //Orient our transform to face where we're going
        if (velocity != Vector3.zero)
        {
            //If he's not facing the way we want, point him towards that way
            transform.forward = velocity.normalized;
        }

        //Keep ourselves grounded JUST IN CASE by using gravity
        velocity.y -= gravity * Time.deltaTime;

        //Call character controller and ask it to move, i.e. ADD VELOCITY TO POSITION
        charController.Move(velocity * Time.deltaTime);
        //gameObject.transform.Translate(velocity * Time.deltaTime);

        //Reset acceleration for next cycle
        acceleration = Vector3.zero;
	}



    //ALL Child classes MUST implement this method
    //Leave commented out for now
    abstract protected void CalcSteeringForces();





	protected void ApplyForce(Vector3 steeringForce)
	{
		//ADD THE FORCE DIVIDED BY THE MASS TO THE ACCELERATION//
        acceleration += steeringForce / mass;
	}


	protected Vector3 Seek (Vector3 targetPosition)
	{
        Vector3 force = Vector3.zero;

        desiredVelocity = targetPosition - transform.position;
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= maxSpeed;
        force = desiredVelocity - velocity;
        force.y = 0;


        //Desired Velocity = Target - Position
        //Normalize desired velocity
        //Multiply desired velocity by maximum speed
        //Steering force = Desired velocity - Vehicle's Velocity

        return force;
	}
    protected Vector3 Arrive(Vector3 targetPosition)
    {
        Vector3 force = Vector3.zero;
        desiredVelocity = targetPosition - transform.position;
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= maxSpeed;
        float decelerationSpeed = Mathf.Pow(Vector3.Distance(targetPosition, gameObject.transform.position), 2) / maxSpeed;
        desiredVelocity *= decelerationSpeed;
        force = desiredVelocity - velocity;
        force.y = 0;
        return force;
    }

    protected Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 force = Vector3.zero;

        desiredVelocity = targetPosition - transform.position;
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= -maxSpeed;
        force = desiredVelocity - velocity;
        force.y = 0;
        //Desired Velocity = Target - Position
        //Normalize desired velocity
        //Multiply desired velocity by the NEGATIVE maximum speed
        //Steering force = Desired velocity - Vehicle's Velocity

        return force;
    }
	protected Vector3 AvoidObstacle(GameObject obst, float obstRad, float safeDistance)
	{
        Vector3 steer = Vector3.zero;
        Vector3 toCenter = obst.transform.position - gameObject.transform.position;
        float dotProdToCenterRight = Vector3.Dot(toCenter, gameObject.transform.right);
        float dotProdToCenterFwd = Vector3.Dot(toCenter, gameObject.transform.forward);
        float distance = Vector3.Magnitude(toCenter);

        //Obstacle obstacleScript = obst.GetComponent<Obstacle>();

        if (distance > safeDistance + obstRad + radius)
        {
            return steer;
        }
        if (dotProdToCenterFwd < 0)
        {
            return steer;
        }
        if (Mathf.Abs(dotProdToCenterRight) > obstRad + radius)
        {
            return steer;
        }

        Vector3 desiredVelocity = Vector3.zero;
        if (dotProdToCenterRight > 0)
        {
            desiredVelocity = gameObject.transform.right * (-maxSpeed * safeDistance / distance);
        }
        else
        {
            desiredVelocity = gameObject.transform.right * (maxSpeed * safeDistance / distance);
        }

        steer = desiredVelocity - velocity;
        return steer;

        //Get vector from the character to the center of the obstacle
        //The float value for the right vector to the center = dot product of VectorToCenter with vehicle's right vector
        //The float value for the distance is the magnitude of this vector
        
        //If (distance > safeDistance + obstacle's radius + vehicle's radius)
            //Return steer
        //If (The dot product of the vector center with the vehicle's forward vector < 0)
            //Return steer
        //If(AbsoluteValue(rightVecToCenter) > obstacle radius + vehicle radius)
            //Return steer
        
        
        //Create a PVector for desiredVelocity
        //If(rightVecToCenter > 0)
            //Set desired velocity by multiplying the vehicle's right vector by the safeDistance/distance scaled by maxSpeed)
        //Else
            //Set desired velocity by multiplying the vehicle's right vector by the safeDistance/distance scaled by NEGATIVE maxSpeed) 
        
        
        //steer = desiredVelocity - vehicle's velocity
	}



    //List is being controlled by GameManager
    //CalcSteeringForce will get centroid from GameManager
    //For separation, I need to know about every other flocker in the flock
	public Vector3 Separation(List<GameObject> flockers, float separationDistance)
	{
        Vector3 total = Vector3.zero;

        foreach (GameObject g in flockers)
        {
            //Get distance between me and each of the neighbors
            desiredVelocity = gameObject.transform.position - g.gameObject.transform.position;
            float dist = desiredVelocity.magnitude;

            //If neighbor is too close
            if (dist > 0 && dist < separationDistance)
            {
                desiredVelocity *= separationDistance / dist;
                desiredVelocity.y = 0;
                total += desiredVelocity;
            }

            
        }


        //Move me in correct direction
        total = total.normalized * maxSpeed;
        total -= velocity;
        return total;

	}
	public Vector3 Alignment(Vector3 alignVector)
	{
        Vector3 steer = Vector3.zero;

        alignVector = alignVector.normalized;
        desiredVelocity = alignVector * maxSpeed;

        steer = desiredVelocity - velocity;
        steer.y = 0;

        return steer;
        //Create a Vector3 that will reference the average vector which is the overall direction
        //Normalize this vector
        //Create a desiredVelocity = averageDirection * maxSpeed
        //Steer = desiredVelocity - vehicle's velocity

        
	}


	public Vector3 Cohesion(Vector3 cohesionVector)
	{
        //GO TO THE CENTER
        return Seek(cohesionVector);

	}
	public Vector3 StayInBounds(float radius, Vector3 center)
	{
        if (Vector3.Distance(gameObject.transform.position, center) > radius)
        {
            return Seek(center);
        }
        else
        {
            return Vector3.zero;
        }
	}


    protected Vector3 PathFollow(List<GameObject> pathNodes)
    {
        
        int closestNodeIndex = pathFollowDetectClosestNodeIndex(pathNodes);
        //Debug.Log(closestNodeIndex);

        GameObject closestNode = pathNodes[closestNodeIndex];
        GameObject nextNode;
        GameObject followingNode;

        

        if (closestNodeIndex + 1 > pathNodes.Count - 1)
        {
            nextNode = pathNodes[0];
        }
        else
        {
            nextNode = pathNodes[closestNodeIndex + 1];
        }

        if (closestNodeIndex + 2 > pathNodes.Count - 1)
        {
            followingNode = pathNodes[1];
        }
        else
        {
            followingNode = pathNodes[closestNodeIndex + 2];
        }
        Vector3 segment = nextNode.transform.position - closestNode.transform.position;

        Debug.DrawLine(closestNode.transform.position, nextNode.transform.position, Color.red);


        Vector3 futurePoint = gameObject.transform.forward * 1.5f;
        Vector3 closestNodeToFuturePoint = futurePoint - pathNodes[closestNodeIndex].transform.position;
        float projectionOfFuturePos = Vector3.Dot(closestNodeToFuturePoint, segment);

        Vector3 desiredVelocity;
        if (projectionOfFuturePos < segment.magnitude)
        {
            desiredVelocity = nextNode.transform.position - gameObject.transform.position;
        }
        else
        {
            desiredVelocity = followingNode.transform.position - gameObject.transform.position;
        }


        //desiredVelocity = targetPosition - transform.position;
        desiredVelocity = desiredVelocity.normalized;

        float decelerationSpeed = Mathf.Pow(Vector3.Distance(nextNode.transform.position, gameObject.transform.position), 2) / maxSpeed;
        
        desiredVelocity *= decelerationSpeed;

        Debug.DrawLine(gameObject.transform.position, desiredVelocity, Color.red);

        Vector3 force = desiredVelocity - velocity;


        return force;
    }


    private int pathFollowDetectClosestNodeIndex(List<GameObject> pathNodes)
    {
        List<float> nodeDistances = new List<float>();
        float winningMag = 0.0f;
        int shortestDistIndex = 0;
        for (int x = 0; x < pathNodes.Count; x++)
        {
            Vector3 distToPlayer = pathNodes[x].transform.position - gameObject.transform.position;
            nodeDistances.Add(distToPlayer.magnitude);
            if (nodeDistances.Count > 1)
            {
                if (nodeDistances[x] < nodeDistances[x - 1] && nodeDistances[x] < winningMag)
                {
                    winningMag = nodeDistances[x];
                    shortestDistIndex = x;
                }

            }
            else
            {
                winningMag = nodeDistances[x];
            }

        }

        return shortestDistIndex;
    }
}
