using UnityEngine;
using System.Collections;

public class PathProximityResolver : MonoBehaviour {

    public static float RecursiveBinarySearch(Vector3 target, Vector3[] path, float min, float max, int clamp) 
    {
        clamp--; //reduces value that tracks number of times the function has been called

        float _mid = min + (max - min) / 2; // sets up mid as equal to minimum value plus half of max minus minimum
        float _product = 0f;

        if (clamp > 0) { //if clamp has not been reduced to 0 and we still are doing calculations
            Vector3 vMin = iTween.PointOnPath(path, min); // sets up a position as equal to the path value at current minimum point
            vMin.y = target.y; //sets the minimum position value's height to be equal to the target's height

            Vector3 vMax = iTween.PointOnPath(path, max); //sets up another position as equal to the currnet max path value
            vMax.y = target.y; //sets this position's height to match the target's height

            if ((target - vMin).sqrMagnitude > (target - vMax).sqrMagnitude) //if the squared distance between the target and the minimum value is greater than that with the maximum
            {
                return RecursiveBinarySearch(target, path, _mid, max, clamp); //run this function again, but narrow it down between the midpoint and the max value as the new min and max
            }
            else
            {
                return RecursiveBinarySearch(target, path, min, _mid, clamp); //run again, but with the minimum as the min and the mid and the new max
            }
        }
        else //if the timer has run out
        {
            return _mid; //return the most recently calculated midpoint
        }
    }
}







