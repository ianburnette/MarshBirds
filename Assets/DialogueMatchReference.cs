using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueMatchReference : MonoBehaviour
{

    public EaseType easeTypeToUse;
    public RectTransform[] elementsToResize, elementsToMatch;
    public float matchSpeed;
    public float[] targetXPos, targetYPos, targetWidth, targetHeight;
    public bool match;
    int i;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Match();
        }
       
    }

    public void Match()
    {
        print("match called for " + transform);
        for (i = 0; i < elementsToResize.Length; i++)
        {
            RectTransform element = elementsToResize[i];
            // print("selected element is " + element + " and matching " + elementsToMatch[i]);

            iTween.ValueTo(element.gameObject, iTween.Hash("from", element.position.x, "to", elementsToMatch[i].position.x, "onupdate", "UpdateXpos", "time", matchSpeed));//, "easetype", easeTypeToUse));
            iTween.ValueTo(element.gameObject, iTween.Hash("from", element.position.y, "to", elementsToMatch[i].position.y,  "onupdate", "UpdateYpos", "time", matchSpeed));//, "easetype", easeTypeToUse));

            iTween.ValueTo(element.gameObject, iTween.Hash("from", element.sizeDelta.x, "to", elementsToMatch[i].sizeDelta.x,  "onupdate", "UpdateWidth", "time", matchSpeed));//, "easetype", easeTypeToUse));
            iTween.ValueTo(element.gameObject, iTween.Hash("from", element.sizeDelta.y, "to", elementsToMatch[i].sizeDelta.y,  "onupdate",  "UpdateHeight", "time", matchSpeed));//, "easetype", easeTypeToUse));

        }
    }
}