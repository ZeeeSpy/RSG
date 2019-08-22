using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject InventoryCanvas;
    
    // Start is called before the first frame update
    void Start()
    {

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
        InventoryCanvas.SetActive(!InventoryCanvas.activeInHierarchy);
    }

    public void ButtonCheck()
    {
        Debug.Log("Button Pressed");
    }
}
