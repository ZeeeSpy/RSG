using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private GameObject currentequipment;
    private InventItemScript currentitemscript;
    private MCMovement playermovement;

    void Start()
    {
        playermovement = (MCMovement)Object.FindObjectOfType(typeof(MCMovement));
    }

    void Update()
    {
        if (currentequipment != null)
        {
            if (Input.GetButtonUp("Use EQP")) //if player uses EQP
            {
                if (currentitemscript.UseItem())
                {
                    GameObject placeditem = Instantiate(currentequipment, transform.position - new Vector3(0, 0.18f, 0), Quaternion.identity);
                }
                else
                {
                    Debug.Log("Ran Out of Equip");
                }
            } else
            {
                Debug.Log("Attemptyed to use null equipment");
            }
        } 
    }


    public void equipitem(GameObject itemtoequip, InventItemScript incscript,string type)
    {
        if (type == "placeable")
        {
            currentequipment = itemtoequip;
            currentitemscript = incscript;
        } 

        if (type == "gun")
        {
            playermovement.GetGun(incscript);
        }
    }
}
