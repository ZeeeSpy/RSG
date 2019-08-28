using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerDoorScript : MonoBehaviour, Interactable
{
    private MCMovement playerscript;
    private IntroText textbox;
    private InventParent invent;

    bool hangerdoor = false;

    void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<MCMovement>();
        textbox = (IntroText)Object.FindObjectOfType(typeof(IntroText));
        invent = (InventParent)Object.FindObjectOfType(typeof(InventParent));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
        if (hangerdoor == false)
        {
            hangerdoor = true;
            textbox.DisplayText("After pressing some buttons you see the hanger door open");
        } else
        {
            hangerdoor = false;
            textbox.DisplayText("After pressing some buttons you see the hanger door close");
        }
    }

    public bool HangarDoorOpen()
    {
        return hangerdoor;
    }
}
