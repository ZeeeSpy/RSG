/*
 *  Script used to change the game music depending on the "alterness" of the guards
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMusicScript : MonoBehaviour
{
    public AudioSource normalmusic;
    public AudioSource alertmusic;
    private bool isalert = false;

    private void Start()
    {
        normalmusic.Play();
    }

    public void startalertmusic()
    {
        if (isalert)
        {
           //do nothing so sound doesn't overlap
        } else
        {
            normalmusic.Stop();
            alertmusic.Play();
            isalert = true;
        }
    }

    public void startnormalmusic()
    {
        if (isalert)
        {
            normalmusic.Play();
            alertmusic.Stop();
            isalert = false;
        }
    }
}
