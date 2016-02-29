using UnityEngine;
using System.Collections;

public class InWaterScript : MonoBehaviour {

    public bool inWater, isPlayer, affectedByWater;
    public float targetHeight;
    public bool diving;
    public Rigidbody rb;
    public float raiseSpeed, diveSpeed;
    public float surfaceDistMargin = 1f;
    public float resetTime = .2f;
    public Vector3 jumpForce;
    public float distToSurface;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (inWater && !diving && affectedByWater)
        {
            if (transform.position.y < targetHeight)
            {
                rb.AddForce(Vector3.up * raiseSpeed * (Mathf.Abs(transform.position.y - targetHeight) + 1f));
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y/(Mathf.Abs(transform.position.y - targetHeight) + 1f), rb.velocity.z);
            }
            if (isPlayer)
            {
                distToSurface = Mathf.Abs(transform.position.y - targetHeight);
                if (distToSurface < surfaceDistMargin && Input.GetButton("Jump"))
                {
                    GetComponent<PlayerMove>().Jump(jumpForce);
                    affectedByWater = false;
                    Invoke("ResetWaterEffect", resetTime);
                }
            }
        }
        if (inWater)
        {
            if (Input.GetButtonDown("Throw"))
                diving = true;
            if (Input.GetButtonUp("Throw"))
                diving = false;
            rb.useGravity = !diving;
            if (diving)
            {
                rb.AddForce(Vector3.down * diveSpeed * Time.deltaTime);
            }
           
        }
        else
        {
            rb.useGravity = true;
            diving = false;
        }
	}

    void ResetWaterEffect()
    {
        affectedByWater = true;
    }

    public void ToggleWater(bool state)
    {
        inWater = state;
    }
}
