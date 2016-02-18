using UnityEngine;
using System.Collections;

public class InventoryMainControl : MonoBehaviour {

    public delegate void DeselectAction(bool state);
    public static event DeselectAction Deselect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Cancel"))
        {
            Deselect(false);
        }
	}
}
