using UnityEngine;
using System.Collections;

public class InventoryMainControl : MonoBehaviour {

    public delegate void DeselectAction(bool state);
    public static event DeselectAction Deselect;
    public PlayerInventory inventoryScript;
    public GameObject[] itemPanels;
    
	void OnEnable() {
        CalculateHeldInventory();
	}

    void CalculateHeldInventory()
    {
        print("calculating inventory");
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
            Deselect(false);
        }
	}
}
