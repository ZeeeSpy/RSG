using UnityEngine;

public class PickUpPlacedMines : MonoBehaviour, Interactable
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
    private MCMovement playerscript;


    public void Start()
    {
        inventparent = (InventParent)Object.FindObjectOfType(typeof(InventParent));
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<MCMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerscript.ToggleInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerscript.ToggleInteractable(null);
        }
    }

    public void Interact()
    {
        inventparent.AddItemToInvent(icon, itemprefab, Itemname, amount, ItemDescription, type);
        Debug.Log("Got " + Itemname);
        Destroy(gameObject); //Destroy self after use
    }
}