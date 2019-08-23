using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite icon;
    public string Itemname;
    public string ItemDescription;
    public GameObject itemprefab;
    private InventParent inventparent;
    public int amount = 1;

    public void Start()
    {
        inventparent = (InventParent)Object.FindObjectOfType(typeof(InventParent));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inventparent.AddItemToInvent(icon, itemprefab, Itemname, amount,ItemDescription);
            Debug.Log("Got " + Itemname);
            Destroy(gameObject); //Destroy self after use
        }
    }

}
