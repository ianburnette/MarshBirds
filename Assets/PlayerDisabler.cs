using UnityEngine;
using System.Collections;

public class PlayerDisabler : MonoBehaviour {

    public PlayerMove moveScript;
    public Throwing throwScript;
    public Health healthScript;
    public PlayerInventory inventoryScript;
    public PlayerLadder ladderScript;
    public PlayerDialogueControls dialogueScript;
    public PlayerCrouch crouchScript;
    public PlayerSlide slideScript;

    public void TogglePlayerScripts( bool state )
    {
        moveScript.enabled = state;
        throwScript.enabled = state;
        healthScript.enabled = state;
        inventoryScript.enabled = state;
        ladderScript.enabled = state;
        dialogueScript.enabled = state;
        crouchScript.enabled = state;
        slideScript.enabled = state;
    }

    public void TogglePlayerScriptsWithoutSlide(bool state)
    {
        moveScript.enabled = state;
        throwScript.enabled = state;
        healthScript.enabled = state;
        inventoryScript.enabled = state;
        ladderScript.enabled = state;
        dialogueScript.enabled = state;
        crouchScript.enabled = state;
    }
}
