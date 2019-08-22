using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite icon;
    public string Itemname;
    public GameObject itemprefab;
    private InventParent inventparent;

    public void Start()
    {
        inventparent = (InventParent)Object.FindObjectOfType(typeof(InventParent));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inventparent.AddItemToInvent(icon, itemprefab, Itemname);
            Debug.Log("Got " + Itemname);
            Destroy(gameObject); //Destroy self after use
        }
    }

}
