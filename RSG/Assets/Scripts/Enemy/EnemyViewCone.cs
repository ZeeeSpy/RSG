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
    Transform viewcone;
    void Start()
    {
        detected = false;
        viewcone = gameObject.transform;
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

    public void NightView()
    {
        viewcone.localScale = new Vector3(1,0.5f,1);
    }

    public void NormalView()
    {
        viewcone.localScale = new Vector3(1, 1, 1);
    }

}
