using UnityEngine;
using System.Collections;

public class portalCollide : MonoBehaviour 
{
    public int levelToLoad = 1;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        portalGen portal = transform.parent.gameObject.GetComponent<portalGen>();
        if (other.gameObject.name == "player" && portal.ActivatePortal)
        {
            Application.LoadLevel(levelToLoad);
        }
    }
}
