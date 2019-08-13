/*
 *  Script that checks for enemies within the "darkness" of a light and tells them to check it out
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchArea : MonoBehaviour
{
    public LightSwitchScript thislightswitch;
    private List<EnemyScript> listofenemies = new List<EnemyScript>();

    public void MakeEnemyCheck(Transform lightocheck)
    {
        bool someonecheking = false;
        for (int i = 0; i < listofenemies.Count; i++)
        {
            if (listofenemies[i] != null)
            {
                if (!someonecheking)
                {
                    listofenemies[i].CheckLightSwitch(lightocheck);
                    someonecheking = true;
                }
                listofenemies[i].NightView();
            }
        }
    }

    public void MakeEnemyHaveNormalVison()
    {
        for (int i = 0; i < listofenemies.Count; i++)
        {
            if (listofenemies[i] != null)
            {
                listofenemies[i].NormalView();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            listofenemies.Add(other.GetComponent<EnemyScript>());
            thislightswitch.SmartLightAI(other.GetComponent<EnemyScript>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            listofenemies.Remove(other.GetComponent<EnemyScript>());
        }
    }
}
