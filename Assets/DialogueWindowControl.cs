using UnityEngine;
using System.Collections;

public class DialogueWindowControl : MonoBehaviour {

    public DialogueImplementation implementation;
    public bool inDialogue;
    public string currentSpeaker, mostRecentSpeaker;
    public Vector3 startVector, endVector;
    private int speakerIndex;
    public RectTransform dialogueBubble, referenceBubble;
    public RectTransform choiceDialogueBubble, choiceReferenceBubble;

    void Start () {
        	
	}
	
	void Update () {
	    if (mostRecentSpeaker != currentSpeaker)    //if speakers have changed 
        {
            mostRecentSpeaker = currentSpeaker;     //update most recent speaker
            DrawNewProcess();                       //make window go to correct place
        }
	}

    void DrawNewProcess()
    {
        implementation.waitForWindow = true;        //pause text typing
        FindSpeakerIndex();                         //find the int index of the current speaker from the npc script
        FindWindowVectors();                        //find the start and end position of the window
        PlaceWindow();                              //place the window in its starting place
    }

    public void DrawChoiceBubble()
    {
        implementation.waitForWindow = true;
        speakerIndex = 0;                           //speaker index should always be zero for choices, so player should always be index 0
        FindWindowVectors();
        PlaceChoiceWindow();
    }

    void PlaceChoiceWindow()
    {
        print("initializing choice window");
        choiceDialogueBubble.localPosition = startVector;
        choiceDialogueBubble.sizeDelta = Vector2.zero;
        choiceReferenceBubble.localPosition = endVector;
        DialogueMatchReference matchRef = choiceDialogueBubble.GetComponent<DialogueMatchReference>();
        matchRef.Match();
        Invoke("ShowChoice", matchRef.matchSpeed);
    }

    public void ChangeChoiceBubble()
    {
        implementation.waitForWindow = true;
        DialogueMatchReference matchRef = choiceDialogueBubble.GetComponent<DialogueMatchReference>();
        matchRef.Match();
        Invoke("ShowChoice", matchRef.matchSpeed);
    }

    void ShowChoice()
    {
        implementation.choiceText.text = implementation.referenceChoiceText.text;
    }

    void FindSpeakerIndex()
    {
        for (int i = 0; i <implementation.npcScript.speakers.Length; i++) 
        {
            string speaker = implementation.npcScript.speakers[i];
            if (currentSpeaker == speaker)
            {
                speakerIndex = i;
                break;
            }
        }
        print("no match found!");
    }
    void FindWindowVectors()
    {
        startVector = implementation.npcScript.speakerBubbleStartPositions[speakerIndex];
        endVector = implementation.npcScript.speakerBubbleEndPositions[speakerIndex];
    }

    void PlaceWindow()
    {
        dialogueBubble.localPosition = startVector;
        dialogueBubble.sizeDelta = Vector2.zero;
        referenceBubble.localPosition = endVector;
        DialogueMatchReference matchRef = dialogueBubble.GetComponent<DialogueMatchReference>();
        matchRef.Match();
        Invoke("BeginTyping", matchRef.matchSpeed);
    }

    void BeginTyping()
    {
        implementation.waitForWindow = false;
    }

    public void HideDialogue()
    {
        dialogueBubble.gameObject.SetActive(false);
    }
    public void HideChoices()
    {
        choiceDialogueBubble.gameObject.SetActive(false);
    }
    public void ShowDialogue()
    {
        dialogueBubble.gameObject.SetActive(true);
    }
    public void ShowChoices()
    {
        choiceDialogueBubble.gameObject.SetActive(true);
    }
}