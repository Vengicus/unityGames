using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour 
{

    public Transform body;

    public Vector3 playerStartPos;

    public float walkSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float jumpHeight = 500.0f;


    private bool moving = false;
    private bool flipping = false;
    private bool jumping = false;
    private bool checkIfJumping = false;


    Vector3 groundPos1;
    Vector3 originPos1;
    Vector3 groundPos2;
    Vector3 originPos2;
    Vector3 groundPos3;
    Vector3 originPos3;
    Vector3 groundPos4;
    Vector3 originPos4;
    Vector3 groundPos5;
    Vector3 originPos5;


    //private level01 levelManager;

    private Quaternion prevRotation;

	// Use this for initialization
	void Start () 
    {
        playerStartPos = gameObject.transform.position;

        animation["Walking"].speed = 3.5f;
        animation["Jump"].speed = 3.5f;
        //animation["Walking"].layer = 5;
        animation["Walking"].AddMixingTransform(body);
        animation["Jump"].AddMixingTransform(body);

        gameObject.rigidbody.angularDrag = 100.0f;

	}
	
	// Update is called once per frame
	void Update () 
    {

        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                animation.Blend("Walking");
                moving = true;

                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate(-Vector3.forward * Time.deltaTime * walkSpeed, Space.Self);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    if (flipping == false)
                    {
                        StartCoroutine("flipAround");
                    }
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Rotate(0, rotationSpeed * Time.deltaTime * 100.0f, 0);
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    transform.Rotate(0, -rotationSpeed * Time.deltaTime * 100.0f, 0);
                }


            }
            else
            {
                moving = false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (isGrounded())
                {
                    StartCoroutine("jump");
                    rigidbody.AddForce(Vector3.up * jumpHeight * 50.75f * Time.deltaTime);
                }
            }
        }
        else
        {
            moving = false;
        }
        

        
        if (moving == false && jumping == false)
        {
            animation.Play("Idle");
        }

        if (isGrounded() && checkIfJumping && jumping)
        {
            jumping = false;
            checkIfJumping = false;
            animation.Play("Idle");
            if (gameObject.transform.rotation != prevRotation)
            {
                gameObject.rigidbody.angularVelocity = new Vector3(0, 0, 0);
                
            }
            
        }
        else if (jumping)
        {
            animation.Blend("Jump");
        }
        
        

        if (flipping)
        {
            Vector3 targ = GameObject.Find("BehindPlayer").transform.position;
            Quaternion targRot = Quaternion.LookRotation(targ - transform.position, Vector3.up);
            targRot.x = 0;
            targRot.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targRot, Time.deltaTime * 2.0f) * Quaternion.Euler(0, 5, 0);
            
        }
        
        prevRotation = transform.rotation;
        
	}

    public bool isGrounded()
    {
        float yOffsetOrigin = 0.5f;
        float yOffsetGround = 0.5f;

        float xOffSet = 0.5f;
        float zOffSet = 0.75f;


        originPos1 = new Vector3(transform.position.x - xOffSet, transform.position.y + yOffsetOrigin, transform.position.z + zOffSet);
        groundPos1 = new Vector3(transform.position.x - xOffSet, transform.position.y - yOffsetGround, transform.position.z + zOffSet);

        originPos2 = new Vector3(transform.position.x + xOffSet, transform.position.y + yOffsetOrigin, transform.position.z + zOffSet);
        groundPos2 = new Vector3(transform.position.x + xOffSet, transform.position.y - yOffsetGround, transform.position.z + zOffSet);

        originPos3 = new Vector3(transform.position.x - xOffSet, transform.position.y + yOffsetOrigin, transform.position.z - zOffSet);
        groundPos3 = new Vector3(transform.position.x - xOffSet, transform.position.y - yOffsetGround, transform.position.z - zOffSet);

        originPos4 = new Vector3(transform.position.x + xOffSet, transform.position.y + yOffsetOrigin, transform.position.z - zOffSet);
        groundPos4 = new Vector3(transform.position.x + xOffSet, transform.position.y - yOffsetGround, transform.position.z - zOffSet);

        originPos5 = new Vector3(transform.position.x, transform.position.y + yOffsetOrigin, transform.position.z);
        groundPos5 = new Vector3(transform.position.x, transform.position.y - yOffsetGround, transform.position.z);

        bool result1 = Physics.Linecast(originPos1, groundPos1, 1 << LayerMask.NameToLayer("ground"));
        bool result2 = Physics.Linecast(originPos2, groundPos2, 1 << LayerMask.NameToLayer("ground"));
        bool result3 = Physics.Linecast(originPos3, groundPos3, 1 << LayerMask.NameToLayer("ground"));
        bool result4 = Physics.Linecast(originPos4, groundPos4, 1 << LayerMask.NameToLayer("ground"));
        bool result5 = Physics.Linecast(originPos5, groundPos5, 1 << LayerMask.NameToLayer("ground"));

        bool grounded = false;
        Color col;
        if (result1 || result2 || result3 || result4 || result5)
        {
            col = Color.green;
            grounded = true;
        }
        else
        {
            col = Color.red;
            grounded = false;
        }
        Debug.DrawLine(originPos1, groundPos1, col, 0.5f, false);
        Debug.DrawLine(originPos2, groundPos2, col, 0.5f, false);
        Debug.DrawLine(originPos3, groundPos3, col, 0.5f, false);
        Debug.DrawLine(originPos4, groundPos4, col, 0.5f, false);
        Debug.DrawLine(originPos5, groundPos5, col, 0.5f, false);
        return grounded;
    }

    IEnumerator flipAround()
    {
        flipping = true;
        yield return new WaitForSeconds(0.5651f);
        flipping = false;
        StopCoroutine("flipAround");
    }

    IEnumerator jump()
    {
        jumping = true;
        yield return new WaitForSeconds(1.2f);
        checkIfJumping = true;
        StopCoroutine("jump");
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "hazard")
        {
            gameObject.transform.position = playerStartPos;
            globalGameManager.playerLives--;

        }
        else if (col.gameObject.name == "fuse")
        {
            globalGameManager.level01FuseCollected = true;
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.name == "portalCollider")
        {
            if (Application.loadedLevel == 1)
            {
                if(globalGameManager.levelsCompleted < 1 && globalGameManager.level01FuseCollected)
                {
                    globalGameManager.levelsCompleted = 1;
                }
            }
            Application.LoadLevel(0);
        }

    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "flockNumber01")
        {
            gameObject.transform.position = playerStartPos;
            globalGameManager.playerLives--;
        }
    }
}
