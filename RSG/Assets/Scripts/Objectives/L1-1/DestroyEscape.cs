/*
 * Script that allows the player to finish level after setting up jet to explode
 */
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
            if (Jet.GetEscape())
            {
                levelfinish.ShowStats(true, false);
            }
        }
    }
}
