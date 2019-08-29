using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetEscape : MonoBehaviour, Interactable
{
    private MCMovement playerscript;

    private IntroText textbox;
    private ControlTowerScript controltower;
    private HangarDoor hangerdoorscript;
    private InventParent inventparent;

    private bool hangerdoor = false;
    private bool clearance = false;
    private bool pilotsnotes = false;

    void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<MCMovement>();
        textbox = (IntroText)Object.FindObjectOfType(typeof(IntroText));
        controltower = (ControlTowerScript)Object.FindObjectOfType(typeof(ControlTowerScript));
        hangerdoorscript = (HangarDoor)Object.FindObjectOfType(typeof(HangarDoor));
        inventparent = (InventParent)Object.FindObjectOfType(typeof(InventParent));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerscript.ToggleInteractable(this);

            //Check objective items
            hangerdoor = hangerdoorscript.HangarDoorOpen();
            clearance = controltower.GotClearance();
            pilotsnotes = inventparent.FindItem("Pilot Notes");
            Debug.Log("Pilot Notes:" + pilotsnotes);
            Debug.Log("Hangar Door:" + hangerdoor);
            Debug.Log("Clearance :" + clearance);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerscript.ToggleInteractable(null);
        }
    }

    public void Interact() //ugly as sin but only called once per interaction so not really that bad. can be optimized later
    {
        // Hanger door, clearance, pilots notes
        if (!hangerdoor && !clearance && !pilotsnotes)
        {
            textbox.DisplayText("Eliminate doesn't necessarily mean 'blow it up'... right?");
        }

        else if (hangerdoor && !clearance && !pilotsnotes)
        {
            textbox.DisplayText("The hanger doors are open, I just gotta figure out how to fly this thing");
        }
        else if (!hangerdoor && clearance && !pilotsnotes)
        {
            textbox.DisplayText("I've got clearance to take off, but the hanger doors are closed and I don't know the take off procedure");
        }
        else if (!hangerdoor && !clearance && pilotsnotes)
        {
            textbox.DisplayText("I know the take off procedure, but the hanger doors are closed");
        }
        else if (!hangerdoor && clearance && pilotsnotes)
        {
            textbox.DisplayText("Got clearance, Got the take off procedure, just need to open the hangar doors");
        }
        else if (hangerdoor && !clearance && pilotsnotes)
        {
            textbox.DisplayText("Bad Ending");
        }
        else if (hangerdoor && clearance && !pilotsnotes)
        {
            textbox.DisplayText("Hanger doors are open and I have clearance, but I still need to figure out how to take off");
        }
        else if (hangerdoor && clearance && pilotsnotes)
        {
            textbox.DisplayText("Good Ending");
        }
    }
}
