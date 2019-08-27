using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class IntroText : MonoBehaviour
{
    private string fullText;
    private string currentText;
    private float delay = 0.1f;
    public Text textbox;
    private string[] ThingsToSay;
    public bool DebugMode;

    private void Update()
    {
        if (Input.GetButtonUp("Submit"))
        {
            GetComponent<Canvas>().enabled = false;
        }
    }

    public void Start()
    {
        if (!DebugMode)
        {
            ThingsToSay = new string[1];
            ThingsToSay[0] = ("Mission Objective:@Eliminate the experimental jet.@@Weapons and equipment are OSP(On - Site Procurement).@@Good luck. Press space to close this window.");
            ThingsToSay[0] = AtToSpace(ThingsToSay[0]);
            StartCoroutine(ShowText(ThingsToSay[0]));
        } else
        {
            GetComponent<Canvas>().enabled = false;
        }
    }

    IEnumerator ShowText(string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textbox.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        
    }

    private string AtToSpace(string stringtochange)
    {
        return stringtochange.Replace("@", System.Environment.NewLine);
    }
}
