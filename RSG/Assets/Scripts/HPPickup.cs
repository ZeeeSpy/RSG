/*
 *  Script used to give player HP on pickup. used only for debugging
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPickup : MonoBehaviour
{
    private MCMovement player;
    public AudioClip pickup;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = (MCMovement)FindObjectOfType(typeof(MCMovement));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(pickup, 1f);
            player.HealPlayer(10);
        }
    }
}
