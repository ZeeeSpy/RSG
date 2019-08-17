/*
 *  Script that handles the logic for turning on/off the lights. allows enemies to turn lights back on
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    private MCMovement playerscript;
    public SpriteRenderer thislight;
    private LightSwitchArea thislightarea;
    public Transform lightswitchposition;
    private bool currentlychecking = false;

    private void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<MCMovement>();
        thislightarea = this.gameObject.GetComponentInChildren<LightSwitchArea>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerscript.ToggleInteractable(this);
        }

        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(TurnLightOn());
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
        if (thislight.enabled == true)
        {
            thislightarea.MakeEnemyCheck(lightswitchposition);
        }
    }

    IEnumerator TurnLightOn()
    {
        yield return new WaitForSeconds(2f);
        thislight.enabled = false;
        thislightarea.MakeEnemyHaveNormalVison();
    }

    public void SmartLightAI(EnemyScript enemytocheck)
    {
        if (thislight.enabled == true && !currentlychecking)
        {
            enemytocheck.CheckLightSwitch(lightswitchposition);
            currentlychecking = true;
            
            //add coroutine that after X amount of time checks if solider is dead
        }
    }

    public bool IsTheLightOn()
    {
        return (thislight.enabled); //false means the "dimness" isn't on
    }
}
