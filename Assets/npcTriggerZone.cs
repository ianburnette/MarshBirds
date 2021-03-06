﻿using UnityEngine;
using System.Collections;

public class npcTriggerZone : MonoBehaviour {

    public GameObject promptUI;
    public Transform tripodTarget;
    public bool inTriggerZone, inDialogue;
    public Transform player;
    public Tripod tripodScript;
    public CustomDialogueImplementation dialogueScript;
    public TextAsset[] myDialogues;
    public int currentDialogueIndex;
    public string[] speakers;
    public Vector2[] speakerBubbleStartPositions;
    public Vector2[] speakerBubbleEndPositions;

    // Use this for initialization
    void Start () {
        promptUI.SetActive(false);
    }
	
    public int GetSpeakerIndex(string speaker)
    {
        for (int i = 0; i< speakers.Length; i++)
        {
            if (speaker == speakers[i])
                return i;
        }
        return 100;
    }

	// Update is called once per frame
	void Update () {
	    if (inTriggerZone && Input.GetButtonDown("Throw") && !inDialogue)
        {
            ToggleMovement(false);
            ToggleCamera(false);
            inDialogue = true;
            tripodScript.referenceTransform = tripodTarget;
            BeginDialogue();
        }
	}

    void BeginDialogue()
    {
        TogglePlayerControls(false);
        dialogueScript.RunDialogueFromNPC(myDialogues[currentDialogueIndex].text, this);
        //print("initiate dialogue here");
        //Invoke("EndDialogue", 2f);
    }

    void TogglePlayerControls(bool state)
    {
        player.GetComponent<PlayerMove>().enabled = state;
        player.GetComponent<Throwing>().enabled = state;
        player.GetComponent<Health>().enabled = state;
        player.GetComponent<PlayerInventory>().enabled = state;
        player.GetComponent<PlayerLadder>().enabled = state;
        player.GetComponent<CharacterMotor>().InDialogue(!state);
    }

    public void EndDialogue()
    {
        inDialogue = false;
        ToggleMovement(true);
        ToggleCamera(true);
        tripodScript.referenceTransform = null;
        TogglePlayerControls(true);
    }

    void ToggleCamera(bool state)
    {
        tripodScript.transform.GetComponent<cameraLookAt>().enabled = state;
        tripodScript.transform.GetComponent<CameraFollowPath>().enabled = state;
        tripodScript.transform.GetComponent<MoveAlongPath>().enabled = state;
    }

    void ToggleMovement(bool state)
    {
        if (player)
        {
            player.GetComponent<PlayerMove>().enabled = state;
            player.GetComponent<PlayerInventory>().enabled = state;
            player.GetComponent<Health>().enabled = state;
            player.GetComponent<Throwing>().enabled = state;
            player.GetComponent<PlayerLadder>().enabled = state;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (player == null)
                player = other.transform;
            promptUI.SetActive(true);
            inTriggerZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            promptUI.SetActive(false);
            inTriggerZone = false;
        }
    }
}
