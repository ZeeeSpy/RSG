using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLineScript : MonoBehaviour
{
    public Transform player;
    private LineRenderer linerenderer;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        player = GameObject.FindWithTag("PlayerBody").GetComponent<Transform>();
        linerenderer = transform.Find("Line").gameObject.GetComponent<LineRenderer>();
        linerenderer.enabled = false;
    }

    void FixedUpdate()
    {

    }

    public bool Hits(RaycastHit2D secondray)
    {
        RaycastHit2D ray = Physics2D.Linecast(this.transform.position, player.position);
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

        if (secondray)
        {
            if (secondray.collider.transform.tag == "PlayerBody")
            {
                linerenderer.SetPosition(0, transform.position);
                linerenderer.SetPosition(1, player.position);
                StartCoroutine(Shoot());
                return true;
            }
        }  else {
            return false;
        }

        linerenderer.SetPosition(0, transform.position);
        linerenderer.SetPosition(1, ray.point);
        StartCoroutine(Shoot());
        return false;
    }

    IEnumerator Shoot()
    {
        linerenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        linerenderer.enabled = false;
    }
}