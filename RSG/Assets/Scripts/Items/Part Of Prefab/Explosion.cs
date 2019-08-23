using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public BoxCollider2D explosionradius;
    private GlobalAlertScript alertscript;
    public AudioClip explosion;
    public AudioSource audioSource;
    public SpriteRenderer thissprite;
    public BoxCollider2D thiscollider;
    bool playingmusic = false;

    private void Start()
    {
        alertscript = (GlobalAlertScript)FindObjectOfType(typeof(GlobalAlertScript));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<EnemyScript>().gethit(5);
        }

        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<MCMovement>().DamagePlayer(5);
        }
        if (!playingmusic)
        {
            playingmusic = true;
            alertscript.GlobalAlertOn();
            audioSource.PlayOneShot(explosion, 7f);
            thissprite.enabled = false;
            thiscollider.enabled = false;
            Destroy(gameObject, 7f);
        }
    }
}
