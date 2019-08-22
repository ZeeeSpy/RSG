using UnityEngine;

public class InventParent : MonoBehaviour
{
    InventItemScript[] InventSlots = new InventItemScript[14];

    void Start()
    {
        int i = 0;
        foreach (Transform child in transform) { 
        
            InventSlots[i] = child.GetComponent<InventItemScript>();
            i++;
        }
    }

    public void AddItemToInvent(Sprite incicon, GameObject incthisgameobject, string itemname, int amount)
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
                InventSlots[i].SetItem(incicon, incthisgameobject, itemname, amount);
                return;
            }
        }
    }
}
