using UnityEngine;
using UnityEngine.UI;

public class InventItemScript : MonoBehaviour
{
    public Image icon;
    private GameObject thisgameobject;
    private string itemname;
    public bool taken = false;
    private int count = 0;
    private InventParent inventparent;
    private string description;

    public void SetItem(Sprite incicon, GameObject incthisgameobject, string incitemname,int amount,string itemdesc)
    {
        taken = true;
        icon.sprite = incicon;
        thisgameobject = incthisgameobject;
        icon.enabled = true;
        itemname = incitemname;
        count = amount;
        description = itemdesc;
    }

    public void IncreaseItem(int amount)
    {
        count = count + amount;
    }

    public void EquipItem()
    {
        inventparent.UpateUI(itemname +": " + count, description ,icon.sprite);
        Debug.Log("Equipping "+itemname + " " + count);
    }

    public bool IsTaken()
    {
        return taken;
    }

    public string GetItemName()
    {
        return itemname;
    }

    public void SetParent(InventParent incscript)
    {
        inventparent = incscript;
    }
}
