using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class CustomDialogueImplementation : MonoBehaviour {

    /*SCOPE OF THIS CLASS
    -types the dialogue into the selected bubble when the bubble is ready
        -has a textToScroll string which is the text it wants to type
    -accepts data from the dialogue class
    -supplies the dialogue class with the default dialogue in the absense of anything else
    -accepts positioning data from the npcTriggerZone script
        -uses this to position the bubbles for animation
    -allows the player to see the choices and transitions between them
        -however, the input should be on the player
        -sends the selected choice back to the dialogue script
    -disables the player's movement scripts when dialogue is entered, and reenables them after
    -controls continuity changes
    */
    #region//VARIABLES
    //choice variables
    public int chosenOption;                            //formerly currentOption, the choice the player chose
    public string[] currentChoiceTexts;                 //array of string of all current choices
    public int currentChoiceMax, currentChoiceIndex;    //current choice count and currently selected
    public bool choiceInputReady;                       //this can accept input from player to change/choose options
    //Dialogue meta-variables
    Dialogue dialogue;                                  //the dialogue base script
    public TextAsset dialogueFileToProcess;             //the current dialogue file
    public string textToRun;                            //the raw text from above
    public Text currentTextToTypeTo;                    //where to type the text
    //physical variables
    public PlayerDialogueControls playerControl;        //the player's control script
    public npcTriggerZone npcScript;                    //the npc script triggering the dialogue
    //UI objects 
    //the visible dialogue display bubbles and thier references
    public Text dialogueBubbleText, dialogueReferenceText, choiceBubbleText, choiceReferenceText;
    public string mostRecentlyDisplayedText;            //the text to check against when drawing a new window
    public GameObject dialogueObject, choiceObject;     //the parent gameobjects for the dialogue components
    public Transform dialogueBaseTransform, choiceBaseTransform;    //the parents that we'll move around
    public Transform dialogueRefTransform, choiceRefTransform;      //the same for the references
    public RectTransform[] dialogueRect, choiceRect;    //the rects to resize
    public RectTransform[] referenceDialogueRect, referenceChoiceRect;   //the rects to match
    //Display control
    public bool waitToType = false;                     //should the script wait to type
    public float transitionTime;                        //how long do movement animations take?
    public float typeTime = 0.5f;                       //how much time between letters
    //window variables
    public string currentSpeaker;                       //who's currently taking
    public string mostRecentSpeaker;                    //last person to talk
    public iTween.EaseType easeTypeToUse;               //the ease type to use when tweening
    //player input
    public bool playerHasPressedNext;                   //if the player pressed next this frame
    #endregion
    #region//INITIALIZATION
    void Awake()
    {
        dialogue = GetComponent<Dialogue>();
        dialogueObject.SetActive(false);
        choiceObject.SetActive(false);
    }
    #endregion
    #region//OPERATIONS
    public void RunDialogueFromNPC(string dialogueNameToRun, npcTriggerZone npcTrigger)
    {
        npcScript = npcTrigger;
        dialogue.Run(dialogueNameToRun);
        //this might need to happen later
        ToggleDialogue(true, dialogueObject);
    }
    void ToggleDialogue(bool state, GameObject dialogueObjectToToggle)
    {
        dialogueObjectToToggle.SetActive(state);
    }
    public IEnumerator EndText()
    {
        npcScript.EndDialogue();
        playerControl.EndDialogue();
        ClearText(currentTextToTypeTo);
        ClearText(dialogueBubbleText);
        ClearText(dialogueReferenceText);
        ClearText(choiceBubbleText);
        ClearText(choiceReferenceText);
        dialogueBubbleText.text = "";
        ToggleDialogue(false, dialogueObject);
        ToggleDialogue(false, choiceObject);
        yield return null;
    }
    void ClearText(Text thisText)
    {
        thisText.text = "";
    }
    public bool InputNext()
    {
        if (playerHasPressedNext)
        {
            playerHasPressedNext = false;
            return true;
        }
        else
            return false;
    }
#endregion
    #region//TEXT DISPLAY
    public IEnumerator Say(string characterName, string text)
    {
        if (!dialogueObject.activeSelf)
        {
            ToggleDialogue(true, dialogueObject);
            ToggleDialogue(false, choiceObject);
        }
        currentTextToTypeTo = dialogueBubbleText;       //we'll type to the dialogue bubble
        ClearText(currentTextToTypeTo);
        dialogueReferenceText.text = text;
        currentSpeaker = characterName;
        //yield return new WaitForSeconds(.5f) ;
        if (currentSpeaker != mostRecentSpeaker)
            DrawNewDialogueBubble(0);
        else
            for (int i = 0; i < dialogueRect.Length; i++)
            {
                
                ResizeRect(dialogueRect[i], referenceDialogueRect[i]);
            }
        do {                                             //hold off until window is displayed
            yield return null;
        } while (waitToType);
        //typing calculations
        float accumTime = 0f;
        int c = 0;
        while (!InputNext() && c < dialogueReferenceText.text.Length)
        {
            yield return null;
            accumTime += Time.deltaTime;
            while (accumTime > typeTime)
            {
                accumTime -= typeTime;
                if (c < dialogueReferenceText.text.Length)
                    currentTextToTypeTo.text += dialogueReferenceText.text[c];
                c++;
            }
        }
        //we're done typing or pressed next
        currentTextToTypeTo.text = dialogueReferenceText.text;
        mostRecentlyDisplayedText = currentTextToTypeTo.text;
        //break out of the coroutine no matter what
        while (InputNext()) yield return null;
        while (!InputNext()) yield return null;
    }
    public IEnumerator RunOptions(List<Dialogue.Option> options)
    {
        currentTextToTypeTo = choiceBubbleText;         //we'll type to the choice bubble
        dialogue.SetCurrentOption(0);                   //make sure no text is currently selected
        yield return null;                              //wait for the dialogue script to realize current option is zero
        ToggleDialogue(false, dialogueObject);          //turn off the normal dialogue object
        ToggleDialogue(true, choiceObject);             //turn on the choice object
        choiceReferenceText.text = "";
        currentChoiceIndex = currentChoiceMax = 0;
        int index = 0;
        foreach (var option in options)
        {
            currentChoiceTexts[index] = option.text;
            index++;
        }
        currentChoiceMax = index - 1;
        choiceReferenceText.text = currentChoiceTexts[currentChoiceIndex];
        for (int i = 0; i < dialogueRect.Length; i++)
            ResizeAndGrowRect(choiceRect[i], referenceChoiceRect[i], 0);
     //   do
      //  {
       //     yield return null;
        //} while (waitToType);
        chosenOption = -1;
        choiceInputReady = true;
        StartCoroutine("ChangeOption", 0);
        do
        {
            yield return null;
            if (choiceInputReady)
                playerControl.GetChoiceInput();
           
        } while (chosenOption == -1);
        ToggleDialogue(false, choiceObject);           //turn off the choice object
        ToggleDialogue(true, dialogueObject);          //turn on the normal dialogue object
        dialogue.SetCurrentOption(chosenOption);       //tell the dialogue that we've chosen something
    }
    public IEnumerator ChangeOption(int optionToChangeTo)
    {
        print("changing option");
        currentChoiceIndex = optionToChangeTo;
        choiceReferenceText.text = currentChoiceTexts[currentChoiceIndex];
        yield return new WaitForSeconds(.02f);
        for (int i = 0; i < choiceRect.Length; i++)
        {
//print("resizing " + choiceRect[i] + " to " + referenceChoiceRect[i] + " and i is " + i); 
            ResizeRect(choiceRect[i], referenceChoiceRect[i]);
        }
        yield return new WaitForSeconds(transitionTime);
        choiceBubbleText.text = choiceReferenceText.text;
        yield return null;
    }
    #endregion
    #region//WINDOW FUNCTIONS
    void DrawNewDialogueBubble(int bubbleType)
    {
        //set positions first
        int speakerIndex = npcScript.GetSpeakerIndex(currentSpeaker);
        dialogueRefTransform.localPosition = npcScript.speakerBubbleEndPositions[speakerIndex];
        dialogueBaseTransform.localPosition = npcScript.speakerBubbleStartPositions[speakerIndex];

        if (bubbleType == 0) //we're drawing a new normal dialogue bubble
        {
            for (int i = 0; i < dialogueRect.Length; i++)
                ResizeAndGrowRect(dialogueRect[i], referenceDialogueRect[i], speakerIndex);
        }
        else //we're drawing a choice bubble
        {
            for (int i = 0; i < choiceRect.Length; i++)
                ResizeRect(choiceRect[i], referenceChoiceRect[i]);
        }

        mostRecentSpeaker = currentSpeaker;
    }
    void ResizeAndGrowRect(RectTransform toResize, RectTransform toMatch, int indexToMatch)
    {
        //set the reference bubble to the correct position
        toMatch.localPosition = new Vector2(npcScript.speakerBubbleEndPositions[indexToMatch].x,
                                      npcScript.speakerBubbleEndPositions[indexToMatch].y);
        toResize.sizeDelta = Vector2.zero;
        waitToType = true;                  //stop the dialogue from typing
        //tween the first to match the second
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.position.x, "to", toMatch.position.x, "onupdate", "UpdateXpos", "time", transitionTime, "easetype", easeTypeToUse));
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.position.y, "to", toMatch.position.y, "onupdate", "UpdateYpos", "time", transitionTime, "easetype", easeTypeToUse));
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.sizeDelta.x, "to", toMatch.sizeDelta.x, "onupdate", "UpdateWidth", "time", transitionTime, "easetype", easeTypeToUse));
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.sizeDelta.y, "to", toMatch.sizeDelta.y, "onupdate", "UpdateHeight", "time", transitionTime, "easetype", easeTypeToUse));
        Invoke("ReadyToType", transitionTime);          //resets the waiting bool in the same length as the tween time
    }
    void ResizeRect(RectTransform toResize, RectTransform toMatch)
    {
        waitToType = true;                  //stop the dialogue from typing
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.sizeDelta.x, "to", toMatch.sizeDelta.x, "onupdate", "UpdateWidth", "time", transitionTime, "easetype", easeTypeToUse));
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.sizeDelta.y, "to", toMatch.sizeDelta.y, "onupdate", "UpdateHeight", "time", transitionTime, "easetype", easeTypeToUse));
        Invoke("ReadyToType", transitionTime);
    }
    void ReadyToType()
    {
        waitToType = false;
    }
    #endregion
    #region //CARRY OVER FUNCTIONS FROM DEFAULT IMPLEMENTATION
    public string Parse(string characterName, string line)
    {
        return line;
    }
    public IEnumerator RunCommand(string line)
    {
        string[] tokens = line.Split(' ');
        if (tokens.Length > 0)
        {
            if (IsString(tokens[0], "wait"))
            {
                float timeToWait = (float)Convert.ToDouble(tokens[1]);
                yield return new WaitForSeconds(timeToWait);
            }
            else if (IsString(tokens[0], "tell"))
            {
                GameObject gameObject = GameObject.Find(tokens[1]);
                if (gameObject != null)
                {
                    int methodToken = 2;
                    if (IsString(tokens[2], "to"))
                        methodToken = 3;

                    string sendData = "";
                    if (tokens.Length > methodToken + 1)
                        sendData = tokens[methodToken + 1];

                    gameObject.SendMessage(tokens[3], sendData, SendMessageOptions.DontRequireReceiver);
                }
            }

        }
        yield break;
    }
    bool ReadBool(string token)
    {
        return IsString(token, "on") || IsString(token, "1");
    }
    bool IsString(string strA, string strB)
    {
        return string.Compare(strA, strB, System.StringComparison.InvariantCultureIgnoreCase) == 0;
    }
    public void SetString(string varName, string varValue)
    {
        // TODO: write this!
    }
    public bool EvaluateIfChunk(string chunk, ref bool result)
    {
        return false;
    }
    #endregion
    #region//CONTINUITY
    public void SetInteger(string varName, int varValue)
    {
        Continuity.instance.SetVar(varName, varValue);
    }
    public int GetInteger(string varName)
    {
        return Continuity.instance.GetVar(varName);
    }
    public void AddToInteger(string varName, int addAmount)
    {
        Continuity.instance.SetVar(varName, Continuity.instance.GetVar(varName) + addAmount);
    }
    #endregion
    #region//ERROR HANDLING
    public void NodeFail()
    {
        print("node not found?");
    }
    #endregion
    #region//PAUSE HANDLING
    public bool IsPaused()
    {
        return false;
    }
    #endregion
}
