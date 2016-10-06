using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Seeker : Vehicle {
    
    //Weight attributes and Distances
    public float seekWeight = 75.0f;
    public float arriveWeight = 10.0f;

    public float avoidWeight = 5.0f;

    public float avoidDist = 4.0f;


    //Reference to the object it is seeking
    public GameObject target;

    private Dictionary<GameObject, float> obstacles;


    //Reference to the GameManager
    private level01 levelManager;


    public float separationDist = 30f;
    public float separationWt = 20.0f;
    public float alignWt = 2.0f;
    public float cohesionWt = 10.0f;
    public float pathWt = 10.0f;



    public List<GameObject> pathNodes = new List<GameObject>();
    public List<Vector3> pathSegments = new List<Vector3>();




	// Use this for initialization
	public override void Start () 
	{
        base.Start();
        obstacles = new Dictionary<GameObject, float>();
        //getPathSegments();
        levelManager = GameObject.Find("levelManager").GetComponent<level01>();


        //obstacles = levelManager.Obstacles;

        
	}
    

    // Update is called once per frame
    void Update() 
	{
        base.Update();
        for (int x = 0; x < pathNodes.Count - 1; x++)
        {
            
        }
	}


    //Sum up all vectors from individual steering forces
    //Use that to guide movement
    protected override void CalcSteeringForces()
	{
        //Start with a zeroed out vector
        Vector3 force = Vector3.zero;
        force += PathFollow(pathNodes) * pathWt;

        force += Separation(levelManager.Flock, separationDist) * separationWt;
        force += Cohesion(levelManager.Centroid) * cohesionWt;
        force += Alignment(levelManager.FlockDir) * alignWt;

        if (obstacles != null)
        {
            //Debug.Log("Avoiding Obstacles");
            //Vector3 averageForces = Vector3.zero;
            foreach (KeyValuePair<GameObject, float> k in levelManager.Obstacles)
            {
                if (k.Key.gameObject.tag == "avoid" && k.Key.gameObject.name != "player")
                {
                    force += AvoidObstacle(k.Key.gameObject, (int)k.Value, avoidDist) * avoidWeight;
                }

            }
        }

        float distanceToPlayer = Vector3.Distance(GameObject.Find("player").transform.position, gameObject.transform.position);
        if(distanceToPlayer <= 8.0f)
        {
            force += Arrive(GameObject.Find("player").transform.position) * arriveWeight;
        }
        if (distanceToPlayer <= 2.0f)
        {
            
            GameObject.Find("player").transform.position = GameObject.Find("player").GetComponent<playerControl>().playerStartPos;
            globalGameManager.playerLives--;
        }
        

        Vector3 centerAvg = Vector3.zero;
        foreach(GameObject g in pathNodes)
        {
            centerAvg += g.transform.position;
        }
        centerAvg /= pathNodes.Count;
        force += StayInBounds(20.0f, centerAvg);


        
        //Limit force by max force float
        force = Vector3.ClampMagnitude(force, maxForce);

        //Apply the OVERALL force
        ApplyForce(force);
	}

    
}



