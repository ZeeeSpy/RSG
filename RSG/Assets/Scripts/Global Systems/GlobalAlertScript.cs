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

    private void Start()
    {
        musicscript = (GlobalMusicScript)Object.FindObjectOfType(typeof(GlobalMusicScript));
    }

    public void GlobalAlertOn()
    {
        Debug.Log("GLOBAL ALERT ON");
        musicscript.startalertmusic();
        alert = true;
    }

    private void GlobalAlertOff()
    {
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

    public void EnemyExit()
    {
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
