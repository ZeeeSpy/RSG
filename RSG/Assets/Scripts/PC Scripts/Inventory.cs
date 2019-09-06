/*
 *  Script used to toggle the inventory panel on and off
 */

using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Canvas InventoryCanvas;
    private InventDescScript descriptionscript;
    
    // Start is called before the first frame update
    void Start()
    {
        descriptionscript = (InventDescScript)Object.FindObjectOfType(typeof(InventDescScript));
        InventoryCanvas = GameObject.FindWithTag("InventoryCanvas").GetComponent<Canvas>(); ;
        InventoryCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Inventory"))
        {
            ToggleInvent();
            descriptionscript.UpdateOnOpen();
        }
    }

    private void ToggleInvent()
    {
        InventoryCanvas.enabled = !InventoryCanvas.enabled;
    }
}
