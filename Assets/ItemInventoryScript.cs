using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInventoryScript : MonoBehaviour {

    public string itemName;
    public bool haveItem, itemSelected;
    public string itemDescription;
    public PlayerInventory playerInventory;
    public GameObject panelGO;
    public Text panelText;
    public GameObject eventSystem;

    void OnEnable()
    {
        InventoryMainControl.Deselect += Select;
        GetComponent<Button>().onClick.AddListener(() => Select(true));
            
    }
    void OnDisable()
    {
        InventoryMainControl.Deselect -= Select;
    }

    void Update()
    {
        
    }

    public void Select(bool state)
    {
        itemSelected = state;
        panelGO.SetActive(state);
        eventSystem.SetActive(!state);
        if (state)
            panelText.text = itemDescription;
        else
            panelText.text = "";
    }
}
