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
    public RectTransform[] dialogueRect, choiceRect;    //the rects to resize
    public RectTransform[] referenceDialogueRect, choiceDialogueRect;   //the rects to match
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

    //INITIALIZATION
    void Awake()
    {
        dialogue = GetComponent<Dialogue>();
        dialogueObject.SetActive(false);
        choiceObject.SetActive(false);
    }

    //OPERATIONS
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
    //TEXT DISPLAY
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
        if (currentSpeaker != mostRecentSpeaker)
            DrawNewDialogueBubble();
        else
            for (int i = 0; i<dialogueRect.Length; i++)
                ResizeRect(dialogueRect[i], referenceDialogueRect[i]);
        do{                                             //hold off until window is displayed
            yield return null;
        } while (waitToType);
        //typing calculations
        float accumTime = 0f;
        int c = 0;
        while(!InputNext() && c < dialogueReferenceText.text.Length)
        {
            yield return null;
            accumTime += Time.deltaTime;
            while(accumTime > typeTime)
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
        ToggleDialogue(false, dialogueObject);
        ToggleDialogue(true, choiceObject);
    }

    //WINDOW FUNCTIONS
    void DrawNewDialogueBubble()
    {
        //redraws the dialogue bubble in correct position for new speaker
        mostRecentSpeaker = currentSpeaker;
    }
    void ResizeRect (RectTransform toResize, RectTransform toMatch)
    {
        //tween the first to match the second
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.position.x, "to", toMatch.position.x, "onupdate", "UpdateXpos", "time", transitionTime, "easetype", easeTypeToUse));
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.position.y, "to", toMatch.position.y, "onupdate", "UpdateYpos", "time", transitionTime, "easetype", easeTypeToUse));
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.sizeDelta.x, "to", toMatch.sizeDelta.x, "onupdate", "UpdateWidth", "time", transitionTime, "easetype", easeTypeToUse));
        iTween.ValueTo(toResize.gameObject, iTween.Hash("from", toResize.sizeDelta.y, "to", toMatch.sizeDelta.y, "onupdate", "UpdateHeight", "time", transitionTime, "easetype", easeTypeToUse));

        Invoke("ReadyToType", transitionTime);          //resets the waiting bool in the same length as the tween time
    }
    void ReadyToType()
    {
        waitToType = false;
    }
    //CARRY OVER FUNCTIONS FROM DEFAULT IMPLEMENTATION
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
    public bool EvaluateIfChunk(string chunk, ref bool result)
    {
        return false;
    }
    //CONTINUITY
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
    //ERROR HANDLING
    public void NodeFail()
    {
        print("node not found?");
    }
    //PAUSE HANDLING
    public bool IsPaused()
    {
        return false;
    }


}
