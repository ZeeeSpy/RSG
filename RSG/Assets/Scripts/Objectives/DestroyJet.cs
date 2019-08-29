using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyJet : MonoBehaviour, Interactable
{
    private MCMovement playerscript;
    private IntroText textbox;
    private InventParent invent;

    private int currentcount = 0;
    private bool workshopman = false;

    void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<MCMovement>();
        invent = (InventParent)Object.FindObjectOfType(typeof(InventParent));
        textbox = (IntroText)Object.FindObjectOfType(typeof(IntroText));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerscript.ToggleInteractable(this);
            currentcount = invent.FindItemCount("C4");
            workshopman = invent.FindItem("Workshop Manual");
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
        if (!workshopman)
        {
            textbox.DisplayText("I could have all the C4 in the world and this reactive armor would brush it off@@ Need to figure out where its weak spots are...");
        } 
        else if (workshopman && currentcount != 4)
        {
            textbox.DisplayText("I need at least 4 bricks of C4 to blow this thing up, I only have " + currentcount);
        }
        else if (workshopman && currentcount == 4)
        {
            textbox.DisplayText("Alright it's all set to blow ,better get the hell out of here");
        }
    }

}
