﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTowerScript : MonoBehaviour, Interactable
{
    private MCMovement playerscript;
    private IntroText textbox;
    private InventParent invent;

    bool scrappaper = false;
    bool clearance = false;

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
            scrappaper = invent.FindItem("Scrap Paper");
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
        if (scrappaper == false)
        {
            textbox.DisplayText("I could try request permission to take off@@ but i don't know the password");
        } else
        {
            textbox.DisplayText("Rquested permission to take off for a 'test run'");
            clearance = true;    
        }
        
    }

    public bool GotClearance()
    {
        return clearance;
    }
}
