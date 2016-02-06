using UnityEngine;
using System.Collections;

public class FreeRoam : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<PlayerMove>().sidescroller = false;
        }
        else if (other.transform.tag == "Pickup")
        {
            other.GetComponent<MoveOnPath>().enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<PlayerMove>().sidescroller = true;
        }
        else if (other.transform.tag == "Pickup")
        {
            other.GetComponent<MoveOnPath>().enabled = true;
        }
    }
}
