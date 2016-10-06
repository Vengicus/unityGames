using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class portalGen : MonoBehaviour 
{

    public float rotationSpeed = 10.0f;
    List<Transform> childObjects;

    public bool activatedPortal = true;

    public bool ActivatePortal
    {
        get
        {
            return activatedPortal;
        }
        set
        {
            activatedPortal = value;
        }
    }


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
        foreach (Transform g in childObjects)
        {
            if (activatedPortal)
            {
                if (g.gameObject.name == "RotatingSpokes")
                {
                    g.gameObject.transform.Rotate(0, rotationSpeed, 0);
                }
                else if (g.gameObject.name == "portalGeneratorGobo"
                        || g.gameObject.name == "portalGobo"
                        || g.gameObject.name == "fuseLight"
                        || g.gameObject.name == "onLight")
                {
                    g.gameObject.light.enabled = true;
                }
                else if (g.gameObject.name == "offLight")
                {
                    g.gameObject.light.enabled = false;
                }
                else if (g.gameObject.name == "Fire_01"
                        || g.gameObject.name == "Fire_02")
                {
                    //g.particleSystem.Play();
                    g.particleSystem.enableEmission = true;
                }
                else if (g.gameObject.name == "portalRays")
                {
                    //g.particleSystem.Play();
                    g.particleSystem.enableEmission = true;
                    
                }
                else if (g.gameObject.name == "Ray"
                           || g.gameObject.name == "Sparkles"
                           || g.gameObject.name == "Ring")
                {
                    //g.particleSystem.Play();
                    g.particleSystem.enableEmission = true;
                }
            }
            else
            {
                if (g.gameObject.name == "portalGeneratorGobo"
                        || g.gameObject.name == "portalGobo"
                        || g.gameObject.name == "fuseLight"
                        || g.gameObject.name == "onLight")
                {
                    g.gameObject.light.enabled = false;
                }
                else if (g.gameObject.name == "offLight")
                {
                    g.gameObject.light.enabled = true;
                }
                else if (g.gameObject.name == "Fire_01"
                        || g.gameObject.name == "Fire_02")
                {
                    //g.particleSystem.Stop();
                    g.particleSystem.enableEmission = false;
                }
                else if (g.gameObject.name == "portalRays")
                {
                    //g.particleSystem.Stop();
                    g.particleSystem.enableEmission = false;
                    
                }
                else if (g.gameObject.name == "Ray"
                           || g.gameObject.name == "Sparkles"
                           || g.gameObject.name == "Ring")
                {
                    //g.particleSystem.Stop();
                    g.particleSystem.enableEmission = false;
                }
            }
        }
	}
}
