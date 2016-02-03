using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {

    public List<GameObject> inventoryItems;
    public int itemsHeld;
    public Throwing throwScript;

    // Use this for initialization
    void Start () {
        throwScript = GetComponent<Throwing>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (itemsHeld>0 && Input.GetButtonDown("Store")){
            RemoveItem(inventoryItems[itemsHeld - 1]);
        }
	}

    public void AddItem(GameObject itemToAdd)
    {
        print("adding item");
        inventoryItems.Add(itemToAdd);
        itemToAdd.SetActive(false);
        itemsHeld++;
    }

    public void RemoveItem(GameObject itemToRemove)
    {
        print("Removing item");
        foreach (GameObject item in inventoryItems)
        {
            if (item == itemToRemove)
            {
                item.SetActive(true);
                throwScript.PickupFromInventory(item.GetComponent<Collider>());
                inventoryItems.Remove(item);
                itemsHeld--;
            }
            break;
        }
    }
}
