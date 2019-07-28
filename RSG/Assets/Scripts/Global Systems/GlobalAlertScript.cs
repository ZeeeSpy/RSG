/*
 *  Script used to alert all guards and to set all guards back to normal mode.
 *  Script also keeps count of number of enemies in map so that player can kill all enemies to stop
 *  alert mode
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAlertScript : MonoBehaviour
{
    private bool alert = false;
    [SerializeField]
    private int enemycount = 0;
    [SerializeField]
    private int lostvisual = 0;
    private GlobalMusicScript musicscript;
    private GlobalPatrolSystem patrolscript;

    private void Start()
    {
        musicscript = (GlobalMusicScript)FindObjectOfType(typeof(GlobalMusicScript));
        patrolscript = (GlobalPatrolSystem)FindObjectOfType(typeof(GlobalPatrolSystem));
    }

    public void GlobalAlertOn()
    {
        Debug.Log("GLOBAL ALERT ON");
        musicscript.startalertmusic();
        alert = true;
    }

    private void GlobalAlertOff()
    {
        patrolscript.Reenforce();
        alert = false;
        musicscript.startnormalmusic();
        lostvisual = 0;
    }

    public bool GetGlobalAlert()
    {
        return alert;
    }

    public void EnemyEnter()
    {
        enemycount++;
    }

    public void EnemyExit(int routenumber)
    {
        patrolscript.DeadGaurdPatrolRoute(routenumber);
        enemycount--;
        if (lostvisual == enemycount)
        {
            GlobalAlertOff();
            lostvisual = 0;
        }
    }

    public void LostVisual()
    {
        lostvisual++;

        if (lostvisual == enemycount)
        {
            GlobalAlertOff();
            lostvisual = 0;
        }
    }

    public void GotVisual()
    {
        lostvisual--;
    }

}
