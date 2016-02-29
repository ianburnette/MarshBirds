using UnityEngine;
using System.Collections;

public class waterScript : MonoBehaviour {

    public float margin;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Pickup")
        {
            other.GetComponent<InWaterScript>().ToggleWater(true);
            other.GetComponent<InWaterScript>().targetHeight = GetComponent<BoxCollider>().bounds.max.y - margin;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Pickup")
        {
            other.GetComponent<InWaterScript>().ToggleWater(false);
        }
    }
}
