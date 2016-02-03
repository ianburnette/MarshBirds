using UnityEngine;
using System.Collections;

public class LadderColliderScript : MonoBehaviour {

    public int myIndex;
    public LadderScript parentLadderScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
            parentLadderScript.TriggerEntered(myIndex, other.transform);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            parentLadderScript.TriggerExited();
    }
}
