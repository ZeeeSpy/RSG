/*
 *  Player item within the inventory, takes one of the 15 slots, can be equiped. interacts with world via invent parent
 */

using UnityEngine;
using UnityEngine.UI;

public class InventItemScript : MonoBehaviour
{
    public Image icon;
    private GameObject thisgameobject;
    private Equipment playerequipment;
    private string itemname;
    private bool Set = false;
    private int count = 0;
    private InventParent inventparent;
    private string description;
    private string type;
    public Sprite NormalInventSlot;
    public Sprite EquipedInventSlot;
    public Image InventSlot;

    private void Start()
    {
        playerequipment = (Equipment)Object.FindObjectOfType(typeof(Equipment));
        InventSlot.sprite = NormalInventSlot;
    }

    public void SetItem(Sprite incicon, GameObject incthisgameobject, string incitemname, int amount, string itemdesc, string inctype)
    {
        Set = true;
        icon.sprite = incicon;
        thisgameobject = incthisgameobject;
        icon.enabled = true;
        itemname = incitemname;
        count = amount;
        description = itemdesc;
        type = inctype;
    }

    public void IncreaseItem(int amount)
    {
        count = count + amount;
    }

    public void EquipItem()
    { 
        if (Set) {
            inventparent.SetItemAsEquiped(this);
            inventparent.UpdateUI(itemname + ": " + count, description, icon.sprite, this);
            //Debug.Log("Equipping " + itemname + " " + count);
            playerequipment.equipitem(thisgameobject, this, type);
            if (GetItemType() != "")
            {
                InventSlot.sprite = EquipedInventSlot;
            }
        }
    }

    public void Unequip()
    {
        InventSlot.sprite = NormalInventSlot;
    }

    public bool IsTaken()
    {
        return Set;
    }

    public string GetItemName()
    {
        return itemname;
    }

    public int getCount()
    {
        return count;
    }

    public void SetParent(InventParent incscript)
    {
        inventparent = incscript;
    }

    public bool UseItem()
    {
        if (count <= 0)
        {
            return false;
        } else
        {
            count--;
            return true;
        }
    }

    public string GetItemType()
    {
        return type;
    }
}
