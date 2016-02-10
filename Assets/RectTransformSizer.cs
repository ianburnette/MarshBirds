using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RectTransformSizer : MonoBehaviour {

    RectTransform myRect;

	void Start () {
        myRect = GetComponent<RectTransform>();
	} 

    void UpdateXpos(float newXpos)
    {
       myRect.position = new Vector2(newXpos, myRect.position.y);
    }

    void UpdateYpos(float newYpos)
    {
        myRect.position = new Vector2(myRect.position.x, newYpos);
    }

    void UpdateWidth(float newWidth)
    {
        myRect.sizeDelta = new Vector2(newWidth, myRect.sizeDelta.y);
        if (transform.name == "choiceTextBox")
            print("setting width to " + newWidth);
    }

    void UpdateHeight(float newHeight)
    {
        myRect.sizeDelta = new Vector2(myRect.sizeDelta.x, newHeight);
    }
}
