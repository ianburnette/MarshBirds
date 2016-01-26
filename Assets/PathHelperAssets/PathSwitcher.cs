using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MoveAlongPath))]
public class PathSwitcher : MonoBehaviour {

    public Transform[] paths;
    public int pathIndex = 0;
    private int currentIndex = 0;
    private string[] pathNames;
    MoveAlongPath pathMovementScript;

	void Start () {
        pathMovementScript = GetComponent<MoveAlongPath>();
        FindPathNames();
	}
	
    void FindPathNames()
    {
        pathNames = new string[paths.Length];
        for (int i = 0; i<paths.Length; i++)
        {
            pathNames[i] = paths[i].GetComponent<iTweenPath>().pathName;
        }
    }

	void Update () {
        GetPlayerInput();
        if (currentIndex!=pathIndex)
            UpdateNames();
	}

    void GetPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            pathIndex++;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            pathIndex--;
        pathIndex = Mathf.Clamp(pathIndex, 0, paths.Length-1);
    }

    void UpdateNames()
    {
        pathMovementScript.pathName = pathNames[pathIndex];
        currentIndex = pathIndex;
    }
}
