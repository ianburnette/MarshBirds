using UnityEngine;
using System.Collections;

public class PlayerDisabler : MonoBehaviour {

    public PlayerMove moveScript;
    public Throwing throwScript;
    public Health healthScript;
    public PlayerInventory inventoryScript;
    public PlayerLadder ladderScript;
    public PlayerDialogueControls dialogueScript;

    public void TogglePlayerScripts( bool state )
    {
        moveScript.enabled = state;
        throwScript.enabled = state;
        healthScript.enabled = state;
        inventoryScript.enabled = state;
        ladderScript.enabled = state;
        dialogueScript.enabled = state;
    }
}
