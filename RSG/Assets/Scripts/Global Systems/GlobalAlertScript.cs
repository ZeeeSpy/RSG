﻿/*
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
    public bool tutorial = false;
    private int alertcount = 0;
    private bool alerttoggle = false;

    private void Start()
    {
        musicscript = (GlobalMusicScript)FindObjectOfType(typeof(GlobalMusicScript));
        patrolscript = (GlobalPatrolSystem)FindObjectOfType(typeof(GlobalPatrolSystem));
    }

    public void GlobalAlertOn()
    {
        if (tutorial)
        {
            Debug.Log("GameOver");
            musicscript.startalertmusic();
            alert = true;
        }
        else
        {
            Debug.Log("GLOBAL ALERT ON");
            musicscript.startalertmusic();
            alert = true;

            if (!alerttoggle)
            {
                alerttoggle = true;
                alertcount++;
            }
        }
    }

    private void GlobalAlertOff()
    {
        patrolscript.Reenforce();
        alert = false;
        musicscript.startnormalmusic();
        alerttoggle = false;
        Debug.Log("GLOBAL ALERT OFF");
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
            if (alert)
            {
                GlobalAlertOff();
                lostvisual = 0;
            }  else
            {
                lostvisual = 0;
            }
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

    public void HardAlertOff()
    {
        lostvisual = 0;
        GlobalAlertOff();
    }

    public void GotVisual()
    {
        lostvisual--;
    }


    public int GetAlertCount()
    {
        return alertcount;
    }
}
