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
            itemPanelButtons = new Button[itemPanels.Length];
            for (int i = 0; i < itemPanelButtons.Length; i++)
            {
                itemPanelButtons[i] = itemPanels[i].GetComponent<Button>();
            }
        }
    }

	void OnEnable() {
        CalculateHeldInventory();
	}
    void OnDisable()
    {
        ResetMenu();
    }

    void CalculateHeldInventory()
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
        if (Input.GetButtonDown("Cancel"))
        {
            ResetMenu();
            SetCurrentButton();
        }
        if (itemSelected && Input.GetButtonDown("Grab"))
        {
            inventoryScript.RemoveItem(inventoryScript.inventoryItems[selectedItemIndex]);
            CloseMenu();
        }
    }

    void ResetMenu()
    {
        Deselect(false);
        itemSelected = false;
        SetButtonEnableState(closeButton, true);
        foreach (Button button in itemPanelButtons)
            SetButtonEnableState(button, true);
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
