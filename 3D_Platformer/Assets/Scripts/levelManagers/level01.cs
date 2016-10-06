using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class level01 : MonoBehaviour {

   



    private List<GameObject> allObstacles;

    private Dictionary<GameObject, float> obstacles;

    public Dictionary<GameObject, float> Obstacles
    {
        get
        {
            return obstacles;
        }
    }


    //Average flock direction
    private Vector3 flockDirection;
    public Vector3 FlockDir
    {
        get
        {
            return flockDirection;
        }
    }

    //Center of the flock
    private Vector3 centroid;
    public Vector3 Centroid
    {
        get
        {
            return centroid;
        }
    }

    //List of flocker objects
    private List<GameObject> flock;
    public List<GameObject> Flock
    {
        get
        {
            return flock;
        }
    }



    

    //Use Mesh.bounds.size to get radius of the object being collided with

	// Use this for initialization
	void Start () 
    {
        //playerStartPos = GameObject.Find("player").transform.position;

        obstacles = new Dictionary<GameObject, float>();


        allObstacles = new List<GameObject>();

        //Get a list of ALL obstacles with colliders in the scene
        foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>())
        {
            if (g.GetComponent<Collider>() != null)
            {
                allObstacles.Add(g);
            }
        }

        //Guarantee that ALL objects will be the same position of radii everytime
        allObstacles.Sort(SortByName);


        //Get ALL the radii
        foreach (GameObject g in allObstacles)
        {
            
            Vector3 radius = g.GetComponent<Collider>().bounds.size;
            float xComp = radius.x;
            float zComp = radius.z;
            float finalRad = 0;
            if (xComp > zComp)
            {
                finalRad = xComp;
            }
            else
            {
                finalRad = zComp;
            }
            //radii.Add(finalRad);
            obstacles.Add(g, finalRad);
        }

        foreach (KeyValuePair<GameObject, float> k in obstacles)
        {
            //Debug.Log(k.Key.name + " | " + k.Value);
        }


        flock = new List<GameObject>();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("flockNumber01"))
        {
            flock.Add(g);
        }
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        foreach (GameObject g in flock)
        {
            /*
            if (Vector3.Distance(g.transform.position, target.transform.position) < 6.0f)
            {
                do
                {
                    target.transform.position = new Vector3(Random.Range(-30, 30), 4f, Random.Range(-30, 30));
                }
                while (NearAnObstacle());

            }
             * */

            g.transform.position = new Vector3(g.transform.position.x, Terrain.activeTerrain.SampleHeight(g.transform.position) + 1.0f, g.transform.position.z);
        }
        //target.transform.position = new Vector3(target.transform.position.x, Terrain.activeTerrain.SampleHeight(target.transform.position) + 4.0f, target.transform.position.z);


        CalcCentroid();
        CalcFlockDirection();
	}

    private static int SortByName(GameObject o1, GameObject o2)
    {
        return o1.name.CompareTo(o2.name);
    }


    private void CalcCentroid()
    {
        centroid = Vector3.zero;
        foreach (GameObject g in flock)
        {
            centroid += g.transform.position;
        }
        centroid /= flock.Count;

        

    }
    private void CalcFlockDirection()
    {
        flockDirection = Vector3.zero;
        foreach (GameObject g in flock)
        {
            flockDirection += g.transform.forward;
        }
        flockDirection /= flock.Count;
        gameObject.transform.forward = flockDirection;
    }
}
