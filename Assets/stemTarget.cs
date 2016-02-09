using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class stemTarget : MonoBehaviour {

    public Vector3 target;
    public float verticalScalingAmount;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);
        transform.localScale = new Vector3(1, Vector3.Distance(target, transform.position) * verticalScalingAmount, 1) ;
	}
}
