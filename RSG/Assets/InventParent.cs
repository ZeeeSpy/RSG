using UnityEngine;

public class InventParent : MonoBehaviour
{
    InventItemScript[] InventSlots = new InventItemScript[12];

    void Start()
    {
        int i = 0;
        foreach (Transform child in transform) { 
        
            InventSlots[i] = child.GetComponent<InventItemScript>();
            i++;
        }
    }

    public void AddItemToInvent(Sprite incicon, GameObject incthisgameobject, string itemname)
    {
        for (int i = 0; i < InventSlots.Length; i++)
        {
            if (!InventSlots[i].IsTaken())
            {
                InventSlots[i].SetItem(incicon, incthisgameobject,itemname);
                return;
            }
        }
    }
}
