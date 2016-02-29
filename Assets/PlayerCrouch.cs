using UnityEngine;
using System.Collections;

public class PlayerCrouch : MonoBehaviour {

    public bool crouching;
    public Transform centralBone; //this will just use animation later
    public float crouchAmt;
    public PlayerMove moveScript;
    public InWaterScript waterScript;
    public float maxSpeed, accel, crouchSpeedDivisor;


	// Use this for initialization
	void Start () {
        maxSpeed = moveScript.maxSpeed;
        accel = moveScript.accel;
	}
	
	// Update is called once per frame
	void Update () {
        if (waterScript.inWater == false)
        {
            WaterCalculations();
        }
        
	}

    void WaterCalculations()
    {
        crouching = Input.GetButton("Crouch");
        if (crouching)
        {
            centralBone.localScale = new Vector3(1, crouchAmt, centralBone.localScale.z);
            moveScript.running = false;
            moveScript.accel = accel / crouchSpeedDivisor;
            moveScript.maxSpeed = maxSpeed / crouchSpeedDivisor;
        }
        else
        {
            centralBone.localScale = Vector3.one;
            moveScript.accel = accel;
            moveScript.maxSpeed = maxSpeed;
        }
    }

    void OnDisable()
    {
        crouching = false;
    }
}
