/*
 *  When the player hits the collision box the item to InventItems via InventParent
 *  multi purpose script can be changed for any pickup that goes in the inventory as well as 
 *  pickups such as ammo
 */

using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite icon;
    public string Itemname;
    [TextArea(10, 20)] //Makes it easier to edit item descriptions. Ugly but w/e
    public string ItemDescription;
    public GameObject itemprefab;
    private InventParent inventparent;
    public int amount = 1;
    public string type;

    public void Start()
    {
        inventparent = (InventParent)Object.FindObjectOfType(typeof(InventParent));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inventparent.AddItemToInvent(icon, itemprefab, Itemname, amount,ItemDescription,type);
            Debug.Log("Got " + Itemname);
            Destroy(gameObject); //Destroy self after use
        }
    }

}
