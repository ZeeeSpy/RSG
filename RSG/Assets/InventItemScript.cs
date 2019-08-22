using UnityEngine;
using UnityEngine.UI;

public class InventItemScript : MonoBehaviour
{
    public Image icon;
    private GameObject thisgameobject;
    private string itemname;
    public bool taken = false;
    private int count = 0;

    public void SetItem(Sprite incicon, GameObject incthisgameobject, string incitemname,int amount)
    {
        taken = true;
        icon.sprite = incicon;
        thisgameobject = incthisgameobject;
        icon.enabled = true;
        itemname = incitemname;
        count = amount;
    }

    public void IncreaseItem(int amount)
    {
        count = count + amount;
    }

    public void EquipItem()
    {
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
}
