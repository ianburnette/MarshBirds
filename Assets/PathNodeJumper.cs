using UnityEngine;
using System.Collections;

public class PathNodeJumper : MonoBehaviour {

    public Transform player;
    public string pathName;
    public int currentPathIndex;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // find player position
        // find player movement direction
        // if moving forward, increment path index and see if new index is more desirable 
        // if so, move to it

        Vector3 test = iTweenPath.GetPath(pathName)[0];
    }
}
