using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {

    public List<GameObject> inventoryItems;
    public List<GameObject> possibleInventoryItems;
    public int itemsHeld;
    public Throwing throwScript;

    // Use this for initialization
    void Start () {
        throwScript = GetComponent<Throwing>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (itemsHeld>0 && Input.GetButtonDown("Store")){
        //    RemoveItem(inventoryItems[itemsHeld - 1]);
        }
	}

    public void AddItem(GameObject itemToAdd)
    {
        //print("adding item");
        for (int i = 0; i<possibleInventoryItems.Count; i++)
        {
            if (itemToAdd.name == possibleInventoryItems[i].name)
                inventoryItems[i] = itemToAdd;
        }
        //inventoryItems.Add(itemToAdd);
        itemToAdd.SetActive(false);
        itemsHeld++;
    }

    public void RemoveItem(GameObject itemToRemove)
    {
        print("Removing item " + itemToRemove.name);
        foreach (GameObject item in inventoryItems)
        {
            if (item != null)
            {
                print("comparing " + itemToRemove + " to " + item.name);
                if (item.name == itemToRemove.name)
                {
                    item.SetActive(true);
                    throwScript.PickupFromInventory(item.GetComponent<Collider>());
                    //inventoryItems.Remove(item);
                    //figure out how to use List predicates to find list item index
                    int indexToRemove = inventoryItems.IndexOf(itemToRemove);
                    inventoryItems[indexToRemove] = null;
                    itemsHeld--;
                    break;
                }
            }
        }
    }
}
