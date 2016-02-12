using UnityEngine;
using System.Collections;

public class PlayerDialogueControls : MonoBehaviour {

    public CustomDialogueImplementation dialogueScript;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        GetChoiceInput();
	}

    void GetChoiceInput()
    {
    
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (currentChoiceIndex >= currentChoiceMax)
                    currentChoiceIndex = 0;
                else
                    currentChoiceIndex++;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (currentChoiceIndex <= 0)
                    currentChoiceIndex = currentChoiceMax;
                else
                    currentChoiceIndex--;
            }
        
       
            if (Input.GetAxis("Horizontal") == 0)
            {
                choiceInputReady = true;
            }
        
        if (referenceChoiceText.text != currentChoiceTexts[currentChoiceIndex])
            UpdateChoiceString();
        if (Input.GetButtonDown("Jump"))
        {
            currentOption = currentChoiceIndex;
        }
    }

    public void EndDialogue()
    {

    }
}
