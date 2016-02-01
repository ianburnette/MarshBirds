using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PathProximityResolver))]
public class PathNodeJumper : MonoBehaviour {

    public Transform objectToSet;
    public string pathName;
    public float distanceMargin;
  [Range(0f, 1f)]
    public float objectPathPosition, forwardPathPosition, backwardPathPosition;
    public GameObject forwardCheckTransform, backwardCheckTransform, smoothedForwardGO, smoothedBackwardGO, movementReferenceSmoothed;
    public float checkTime, smootheTime;
    public float transformSmoothingTime;
    public float pathProximityMargin;
    public int solverTimer;
    public bool loop;

    PathProximityResolver pathSolver;

    void Start () {
        pathSolver = GetComponent<PathProximityResolver>();
        InvokeRepeating("FindPositionOnPath", 0, checkTime);
    }

    void Update()
    {
        CalculateSurroundingValues();
        ClampValues();
        SetTransformToPath(gameObject, objectPathPosition);
        SetTransformToPath(forwardCheckTransform, forwardPathPosition);
        SetTransformToPath(backwardCheckTransform, backwardPathPosition);
        SetSmoothedTransforms(movementReferenceSmoothed.transform, transform);
        SetSmoothedTransforms(smoothedForwardGO.transform, forwardCheckTransform.transform);
        SetSmoothedTransforms(smoothedBackwardGO.transform, backwardCheckTransform.transform);
        DebugRay();
    }

    void DebugRay()
    {
        Debug.DrawRay(transform.position, Vector3.down * 100, Color.yellow);
    }

    void ClampValues()
    {
        if (loop)
        {
            LoopValueSet(objectPathPosition);
            LoopValueSet(forwardPathPosition);
            LoopValueSet(backwardPathPosition);       
        }
        Mathf.Clamp(objectPathPosition, 0f, 1f);
        Mathf.Clamp(forwardPathPosition, 0f, 1f);
        Mathf.Clamp(backwardPathPosition, 0f, 1f);
    }

    float LoopValueSet (float toSet)
    {
        float revised = 0f;
        if (toSet > 1f)
            revised -= 1f;
        if (toSet < 0f)
            revised = 1f + toSet;
        return revised;
    }

    void SetTransformToPath(GameObject toSet, float position)
    {
        //  print("placing at " + position);
        if (position < 0f)
            position = 0f;
        if (position > 1f)
            position = 1f;
        iTween.PutOnPath(toSet, iTweenPath.GetPath(pathName), position);
    }

    void SetSmoothedTransforms(Transform toMove, Transform reference)
    {
        toMove.transform.position = Vector3.Lerp(toMove.transform.position, reference.transform.position, transformSmoothingTime * Time.deltaTime);
    }

    void CalculateSurroundingValues()
    {
        forwardPathPosition = objectPathPosition + distanceMargin;
        backwardPathPosition = objectPathPosition - distanceMargin;
        Mathf.Clamp(forwardPathPosition, 0f, 1f);
        Mathf.Clamp(backwardPathPosition, 0f, 1f);
    }

    void FindPositionOnPath()
    {
        float desiredPathLocation = PathProximityResolver.RecursiveBinarySearch(objectToSet.position, iTweenPath.GetPath(pathName), objectPathPosition - pathProximityMargin, objectPathPosition + pathProximityMargin, solverTimer);
        objectPathPosition = Mathf.Lerp(objectPathPosition, desiredPathLocation, smootheTime * Time.deltaTime);
        ClampValues();
    }
}
