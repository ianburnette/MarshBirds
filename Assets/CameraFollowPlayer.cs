using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour {

    public Transform player, heightReference, forwardRef, backwardRef;

    public float camDistance, camAdjustSpeed;

    public PlayerMove moveScript;

    Vector3 midpoint, camDestination;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        FollowPlayer();
	}

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, heightReference.transform.position.y, player.transform.position.z);

        Vector3 lhs, rhs;

        midpoint = new Vector3(heightReference.position.x, player.position.y, heightReference.position.z);
        if (moveScript.publicMovementVector.x >= 0)
            camDestination = midpoint + heightReference.right * camDistance;
        else
            camDestination = midpoint - heightReference.right * camDistance;
        camDestination = new Vector3(camDestination.x, heightReference.position.y, camDestination.z);
        
        transform.position = Vector3.Lerp(transform.position, camDestination, camAdjustSpeed * Time.deltaTime);

        lhs = heightReference.position;
        rhs = player.position;
        Debug.DrawRay(lhs, rhs - lhs, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.Cross(lhs, rhs), Color.red);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(camDestination, 3f);
    }
}
