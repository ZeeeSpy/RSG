/*
 *  all child objects are InventItem's. Has an array of all child objects, in charge of 
 *  assigning empty slots to picked up items and updating invent items values as well as 
 *  invent items interfacing with the world
 */

using UnityEngine;

public class InventParent : MonoBehaviour
{
    InventItemScript[] InventSlots = new InventItemScript[15]; //magic number array size :/
    public InventDescScript descriptor;

    void Start()
    {
        InventItemScript temp;
        int i = 0;
        foreach (Transform child in transform) { 
        
            InventSlots[i] = child.GetComponent<InventItemScript>();
            temp = child.GetComponent<InventItemScript>();
            temp.SetParent(this);
            i++;
        }
    }

    public void AddItemToInvent(Sprite incicon, GameObject incthisgameobject, string itemname, int amount, string itemdesc)
    {
        // loop through items, if taken check if its the same, if it is increase, else keep looping
        // if the slot is empty add item
        for (int i = 0; i < InventSlots.Length; i++)
        {
            if (InventSlots[i].IsTaken())
            {
                if (InventSlots[i].GetItemName() == itemname)
                {
                    InventSlots[i].IncreaseItem(amount);
                    return;
                }
            } else
            {
                InventSlots[i].SetItem(incicon, incthisgameobject, itemname, amount, itemdesc);
                return;
            }
        }
    }

    public void UpateUI(string itemname,string itemdesc, Sprite itemimage)
    {
        descriptor.UpdateUI(itemname, itemdesc, itemimage);
    }
}
