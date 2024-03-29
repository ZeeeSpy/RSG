﻿/*
 * Used to show a dialog box that infroms player of player characters thoughts while interacting.
 * closed with "submit"
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class IntroText : MonoBehaviour
{
    private string fullText;
    private string currentText;
    private float delay = 0.01f;
    public Text textbox;
    private string ThingsToSay;
    public bool DebugMode;
    private InventDescScript descriptor;

    private void Update()
    {
        if (Input.GetButtonUp("Submit"))
        {
            GetComponent<Canvas>().enabled = false;
        }
    }

    public void Start()
    {
        descriptor = (InventDescScript)Object.FindObjectOfType(typeof(InventDescScript));
        if (!DebugMode)
        {
            ThingsToSay  = ("Mission Objective:@Eliminate the experimental jet.@@Weapons and equipment are OSP(On - Site Procurement).@@Good luck. Press space to close this window.");
            StartCoroutine(ShowText(ThingsToSay));
        } else
        {
            GetComponent<Canvas>().enabled = false;
        }
    }

    IEnumerator ShowText(string fullText)
    {
        fullText = AtToSpace(fullText);
        GetComponent<Canvas>().enabled = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textbox.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        
    }

    public void DisplayText(string texttoshow)
    {
        StopAllCoroutines();
        descriptor.UpdateCurrentObjective(texttoshow);
        StartCoroutine(ShowText(texttoshow));
    }

    private string AtToSpace(string stringtochange)
    {
        return stringtochange.Replace("@", System.Environment.NewLine);
    }
}
