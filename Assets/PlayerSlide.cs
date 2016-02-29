using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerSlide : MonoBehaviour {

    public float forwardForce, slideDrag;
    public bool sliding, canGetUp;
    public PlayerMove moveScript;
    public PlayerDisabler disablerScript;
    public Throwing throwScript;
    public MoveOnPath pathMover;
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
	
	}
	
    void BeginSlide(float horizontalInput)
    {
        sliding = true;
        rb.drag = slideDrag;
        disablerScript.TogglePlayerScriptsWithoutSlide(false);
        pathMover.enabled = true;
        Vector3 forwardPos = new Vector3(pathMover.movementRefForward.position.x, transform.position.y, pathMover.movementRefForward.position.z);
        Vector3 backwardPos = new Vector3(pathMover.movementRefBackward.position.x, transform.position.y, pathMover.movementRefBackward.position.z);
        Debug.DrawRay(transform.position, transform.position - forwardPos, Color.green);
        Debug.DrawRay(transform.position, transform.position - backwardPos, Color.blue);
        //EditorApplication.isPaused = true;
        if (horizontalInput > 0f) 
            rb.AddForce((transform.position - backwardPos) * forwardForce);
        else
            rb.AddForce((transform.position - forwardPos) * forwardForce);
    }

    void EndSlide()
    {
        sliding = false;
        rb.drag = 0f;
        pathMover.enabled = false;
        disablerScript.TogglePlayerScriptsWithoutSlide(true);

    }

    void OnDisable()
    {
        EndSlide();
    }

	// Update is called once per frame
	void Update () {
        if (moveScript.enabled && throwScript.heldObj == null) //can move and not on ladder and not holding anything
        {
            if (!moveScript.grounded) //not on the ground
            {
                if (rb.velocity.y > 0) // moving up
                {
                    float h = Input.GetAxis("Horizontal");
                    if (Input.GetButtonDown("Jump") && h != 0f)
                    {
                        BeginSlide(h);
                    }
                }
            }
        }else if (!moveScript.enabled && Input.GetButtonDown("Jump")) // && on ground
        {
            EndSlide();
        }
	    
	}
}
