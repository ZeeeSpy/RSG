/*
 *  Script used to toggle the inventory panel on and off
 */

using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Canvas InventoryCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        InventoryCanvas = GameObject.FindWithTag("InventoryCanvas").GetComponent<Canvas>(); ;
        ToggleInvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Inventory"))
        {
            ToggleInvent();
        }
    }

    private void ToggleInvent()
    {
        InventoryCanvas.enabled = !InventoryCanvas.enabled;
    }
}
