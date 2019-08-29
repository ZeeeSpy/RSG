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

    private void Update()
    {
        if (Input.GetButtonUp("Submit"))
        {
            GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
        }
    }

    public void Start()
    {
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
        StartCoroutine(ShowText(texttoshow));
    }

    private string AtToSpace(string stringtochange)
    {
        return stringtochange.Replace("@", System.Environment.NewLine);
    }
}
