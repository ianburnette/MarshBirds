using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InventoryMainControl : MonoBehaviour {

    public delegate void DeselectAction(bool state);
    public static event DeselectAction Deselect;
    public EventSystem eventSys;
    public PlayerInventory inventoryScript;
    public GameObject[] itemPanels;
    public Button[] itemPanelButtons;
    public ItemInventoryScript[] itemScripts;
    public Button closeButton;
    public GameObject removeItemObject, descriptionPanel;
    public Button lastSelectedButton;
    public PauseScript pauser;
    public bool itemSelected;
    public int selectedItemIndex;
    
    void Start()
    {
        if (itemPanelButtons.Length == 0)
        {
            itemPanelButtons = new Button[itemPanels.Length];                   //create an array to hold all held item buttons based on the panel array
            for (int i = 0; i < itemPanelButtons.Length; i++)                   //for each of these buttons
                itemPanelButtons[i] = itemPanels[i].GetComponent<Button>();     //add it to the array of buttons we just created
        }
    }

	void OnEnable() {
        CalculateHeldInventory();                                               //update to see if we have any new items
	}
    void OnDisable()
    {
        ResetMenu();                                                            //return menu to its default state
    }
    void CalculateHeldInventory()                                               //check each item in inventory against panels to display, and hide/show them appropriately
    {
        for (int i = 0; i<inventoryScript.inventoryItems.Count; i++)
        {
            if (inventoryScript.inventoryItems[i] != null)
            {
                itemPanels[i].SetActive(true);
                itemPanels[i].GetComponent<ItemInventoryScript>().haveItem = true;
            }
            else
                itemPanels[i].SetActive(false);
            itemPanels[i].GetComponent<ItemInventoryScript>().haveItem = false;
        }
    }

	void Update () {
        if (Input.GetButtonDown("Cancel"))                                      //handle going back in the menu
        {
            ResetMenu();
            SetCurrentButton();
            //also make it exit the inventory screen? try this to see how it feels
        }
        if (itemSelected && Input.GetButtonDown("Grab"))                        //take the item out of your inventory and put it in your hand
        {
            inventoryScript.RemoveItem(inventoryScript.inventoryItems[selectedItemIndex]);
            CloseMenu();
        }
    }

    void ResetMenu()
    {
        
        itemSelected = false;
        SetButtonEnableState(closeButton, true);
        foreach (Button button in itemPanelButtons)
            SetButtonEnableState(button, true);

        if (Deselect!=null)
            Deselect(false);
    }

    public void CloseMenu()
    {
        ResetMenu();
        pauser.Pause();
    }

    public void SelectButton(int itemIndex)
    {
        lastSelectedButton = itemPanelButtons[itemIndex];
        SetButtonEnableState(closeButton, false);
        itemScripts[itemIndex].Select(true);
        foreach (Button button in itemPanelButtons)
            SetButtonEnableState(button, false);
        itemSelected = true;
        selectedItemIndex = itemIndex;
    }

    void SetCurrentButton()
    {
        eventSys.SetSelectedGameObject(lastSelectedButton.gameObject);
    }

    void SetButtonEnableState(Button buttonToChange, bool state)
    {
        buttonToChange.interactable = state;
    }
}
