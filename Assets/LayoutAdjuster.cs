using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class LayoutAdjuster : MonoBehaviour {

    public Text myText;
    public int charLimit, currentCharCount;
    public LayoutElement layout;

	void Start () {
	    if (myText== null)
            myText = GetComponent<Text>();
        if (layout == null)
            layout = GetComponent<LayoutElement>();
	}
	
	void Update () {
        currentCharCount = myText.text.Length;
        if (currentCharCount > charLimit && layout.enabled == false)
        {
            layout.enabled = true;
        }
        else if (currentCharCount <= charLimit && layout.enabled == true)
            layout.enabled = false;
    }
}
