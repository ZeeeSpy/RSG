using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private GameObject currentequipment;
    private InventItemScript currentitemscript;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetButtonUp("Use EQP")) //if player uses EQP
        {
            if (currentitemscript.UseItem())
            {
                GameObject placeditem = Instantiate(currentequipment, transform.position - new Vector3(0, 0.18f, 0), Quaternion.identity);
            } else
            {
                Debug.Log("Ran Out of Equip");
            }
        }
    }


    public void equipitem(GameObject itemtoequip, InventItemScript incscript)
    {
        currentequipment = itemtoequip;
        currentitemscript = incscript;
    }
}
