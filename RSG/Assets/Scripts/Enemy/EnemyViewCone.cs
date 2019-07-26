/*
 *  Enemy view cones are poly coliders, this is the script that detects if a player is in it.
 *  if the enemy can raycast an uninterrupted line to the player and they are within the cone that means
 *  the player is seen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewCone : MonoBehaviour
{
    bool detected; 
    void Start()
    {
        detected = false;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBody")
        {
            detected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBody")
        {
            detected = false;
        }
    }

    public bool isDetected()
    {
        return detected;
    }

}
