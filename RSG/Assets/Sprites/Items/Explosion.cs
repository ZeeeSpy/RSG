using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public BoxCollider2D explosionradius;
    public GlobalAlertScript alertscript;

    private void Start()
    {
        alertscript = (GlobalAlertScript)FindObjectOfType(typeof(GlobalAlertScript));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Got To Here");
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<EnemyScript>().gethit(5);
        }

        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<MCMovement>().DamagePlayer(5);
        }
        alertscript.GlobalAlertOn();
        Destroy(gameObject, 0.2f);
    }
}
