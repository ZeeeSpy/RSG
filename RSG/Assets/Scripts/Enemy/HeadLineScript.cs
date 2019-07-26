/*
 *  Unused script: this script was used before to fix the depth problem by having the enenmy check for the 
 *  player from two places on the enemy (head, toes) this was changed to 4 places on the player. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLineScript : MonoBehaviour
{
    public Transform player;
    private LineRenderer linerenderer;
    private LayerMask enemyignoremask;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        player = GameObject.FindWithTag("PlayerBody").GetComponent<Transform>();
        linerenderer = transform.Find("Line").gameObject.GetComponent<LineRenderer>();
        linerenderer.enabled = false;
        enemyignoremask = LayerMask.NameToLayer("EnemyIgnore");
        enemyignoremask = ~enemyignoremask;
    }

    void FixedUpdate()
    {

    }

    public bool Hits(bool Secondhit)
    {
        RaycastHit2D ray = Physics2D.Linecast(this.transform.position, player.position, enemyignoremask);
        //Debug.DrawLine(transform.position, ray.point, Color.black);
        if (ray)
        {
            if (ray.collider.transform.tag == "PlayerBody")
            {
                linerenderer.SetPosition(0, transform.position);
                linerenderer.SetPosition(1, player.position);
                StartCoroutine(Shoot());
                return true;
            }
        } else {
            return false;
        }

        if (Secondhit)
        {
                linerenderer.SetPosition(0, transform.position);
                linerenderer.SetPosition(1, player.position);
                StartCoroutine(Shoot());
                return true;

        }  else {
            linerenderer.SetPosition(0, transform.position);
            linerenderer.SetPosition(1, ray.point);
            StartCoroutine(Shoot());
            return false;
        }

        
    }

    IEnumerator Shoot()
    {
        linerenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        linerenderer.enabled = false;
    }
}