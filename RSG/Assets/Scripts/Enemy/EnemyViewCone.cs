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
    PolygonCollider2D viewcone;
    void Start()
    {
        detected = false;
        viewcone = gameObject.GetComponent<PolygonCollider2D>();
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
        var myPoints = viewcone.points;
        myPoints[1] = new Vector2(0.5f, -0.7f);
        myPoints[2] = new Vector2(-0.5f, -0.7f);
        viewcone.points = myPoints;
    }

    public void NormalView()
    {
        var myPoints = viewcone.points;
        myPoints[1] = new Vector2(0.5f, -1.3f);
        myPoints[1] = new Vector2(-0.5f, -1.3f);
        viewcone.points = myPoints;
    }

}
