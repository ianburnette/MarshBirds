using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class choiceToggleSpacer : MonoBehaviour {

    public Text referenceText;
    HorizontalLayoutGroup myLayoutGroup;
    public float sizeDivisor;

	void Start () {
        myLayoutGroup = GetComponent<HorizontalLayoutGroup>();
	}
	
	void Update () {
        myLayoutGroup.spacing = -(referenceText.rectTransform.sizeDelta.x - (Mathf.Sqrt(referenceText.rectTransform.sizeDelta.x) / sizeDivisor));
    }
}
