using UnityEngine;
using System.Collections;

public class PlayerDialogueControls : MonoBehaviour {

    public CustomDialogueImplementation dialogueScript;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        GetNextInput();
       // GetChoiceInput();
	}

    void GetNextInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            dialogueScript.playerHasPressedNext = true;
        }
        if (!dialogueScript.choiceInputReady)
        {
            CheckForReset();
        }
    }

    public void GetChoiceInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (dialogueScript.currentChoiceIndex >= dialogueScript.currentChoiceMax)
                ChangeOption(0);//dialogueScript.currentChoiceIndex = 0;
            else
                ChangeOption(dialogueScript.currentChoiceIndex+1);
            dialogueScript.choiceInputReady = false;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (dialogueScript.currentChoiceIndex <= 0)
                ChangeOption(dialogueScript.currentChoiceMax); //dialogueScript.currentChoiceIndex = dialogueScript.currentChoiceMax;
            else
                ChangeOption(dialogueScript.currentChoiceIndex - 1);//dialogueScript.currentChoiceIndex--;
            dialogueScript.choiceInputReady = false;
        }
    
        if (Input.GetButtonDown("Jump"))
        {
            dialogueScript.chosenOption = dialogueScript.currentChoiceIndex;
        }
    }

    void ChangeOption(int newOption)
    {
        dialogueScript.StartCoroutine("ChangeOption", newOption);
    }

    void CheckForReset()
    {
        if (Input.GetAxis("Horizontal") == 0)
        {
            dialogueScript.choiceInputReady = true;
        }
    }

    public void EndDialogue()
    {

    }
}
