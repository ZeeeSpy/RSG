using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEscape : MonoBehaviour
{
    private DestroyJet Jet;
    private FinishLevel levelfinish;

    private void Awake()
    {
        Jet = (DestroyJet)Object.FindObjectOfType(typeof(DestroyJet));
        levelfinish = (FinishLevel)Object.FindObjectOfType(typeof(FinishLevel));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Got Here");
            if (Jet.GetEscape())
            {
                Debug.Log("Got Here 2");
                levelfinish.ShowStats(true, false);
            }
        }
    }
}
