using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rotateWings : MonoBehaviour 
{
    public float rotationSpeed = 10.0f;
    List<Transform> childObjects;
	// Use this for initialization
	void Start () 
    {
        childObjects = new List<Transform>();
        foreach (Transform t in transform)
        {
            childObjects.Add(t);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        foreach (Transform t in childObjects)
        {
            if (t.name == "upperWings" | t.name == "lowerWings")
            {
                t.transform.Rotate(0, rotationSpeed, 0);
            }
        }
	}
}
