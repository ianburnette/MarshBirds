using UnityEngine;
using System.Collections;

public class FreeRoam : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<PlayerMove>().sidescroller = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<PlayerMove>().sidescroller = true;
        }
    }
}
