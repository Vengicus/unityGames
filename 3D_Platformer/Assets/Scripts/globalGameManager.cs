using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class globalGameManager : MonoBehaviour 
{

    public static List<GameObject> portalGenerators;
    public List<portalGen> portalScripts;


    public static int levelsCompleted = 0;

    
    public static int playerLives = 3;
    public static int playerHP = 100;


    public static bool level01FuseCollected = false;
    public static bool level02FuseCollected = false;
    public static bool level03FuseCollected = false;
    public static bool level04FuseCollected = false;


	// Use this for initialization
	void Start () 
    {
        portalGenerators = new List<GameObject>();
        if (Application.loadedLevel == 0)
        {
            if (portalGenerators.Count == 0)
            {
                foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>())
                {
                    if (g.tag == "portalGenerator")
                    {
                        portalGenerators.Add(g);
                    }
                }
            }
            if (levelsCompleted == 0)
            {

            }
            
            foreach (GameObject g in portalGenerators)
            {
                Debug.Log(g.name);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Application.loadedLevel == 0)
        {
            if (levelsCompleted >= 1)
            {
                GameObject.Find("portalGenerator02").GetComponent<portalGen>().ActivatePortal = true;
            }
        }
        else
        {
            //Debug.Log(playerLives);
            if (playerLives <= 0)
            {
                playerLives = 3;
                Application.LoadLevel(0);
            }
            
        }
	}
    /*
    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            if (levelsCompleted == 0)
            {

            }
            else if (levelsCompleted >= 1)
            {
                portalGenerators[1].GetComponent<portalGen>().ActivatePortal = true;
            }
        }
    }*/



}
