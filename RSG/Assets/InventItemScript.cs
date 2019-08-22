using UnityEngine;
using UnityEngine.UI;

public class InventItemScript : MonoBehaviour
{
    public Image icon;
    private GameObject thisgameobject;
    private string itemname;
    public bool taken = false;

    public void SetItem(Sprite incicon, GameObject incthisgameobject, string incitemname)
    {
        taken = true;
        icon.sprite = incicon;
        thisgameobject = incthisgameobject;
        icon.enabled = true;
        itemname = incitemname;
    }

    public void EquipItem()
    {
        Debug.Log("Equipping "+itemname);
    }

    public bool IsTaken()
    {
        return taken;
    }
}
