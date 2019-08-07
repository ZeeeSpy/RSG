using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    private MCMovement playerscript;
    public SpriteRenderer thislight;

    private void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<MCMovement>();

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

    public void Togglelight()
    {
        thislight.enabled = !thislight.enabled;
    }
}
