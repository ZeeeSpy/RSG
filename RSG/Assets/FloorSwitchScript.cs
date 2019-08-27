using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitchScript : MonoBehaviour, Interactable
{
    public Transform otherspace;
    public GameObject cameratodisable;
    public GameObject cameratoenable;
    private MCMovement playerscript;
    private Transform player;
    private GlobalAlertScript alertscript;

    private void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<MCMovement>();
        alertscript = (GlobalAlertScript)Object.FindObjectOfType(typeof(GlobalAlertScript));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerscript.ToggleInteractable(this);
            player = collision.transform;
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
        player.transform.position = otherspace.position;
        cameratodisable.SetActive(false);
        cameratoenable.SetActive(true);
        Debug.Log("Moved Player");
        alertscript.HardAlertOff();
    }
}
