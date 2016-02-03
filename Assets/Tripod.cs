using UnityEngine;
using System.Collections;

public class Tripod : MonoBehaviour {

    public Camera cam;
    public float smoothingSpeed;
    public float baseFieldOfView;
    public Transform referenceTransform;

	void Update () {
        if (referenceTransform)
        {
            transform.position = Vector3.Lerp(transform.position, referenceTransform.position, smoothingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, referenceTransform.rotation, smoothingSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, referenceTransform.localScale.x, smoothingSpeed * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFieldOfView, smoothingSpeed * Time.deltaTime);
        }
	}
}
