using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }
    void FixedUpdate()
    {
        RaycastHit2D ray = Physics2D.Linecast(this.transform.position, player.position);

        //see line for debugging
        Debug.DrawLine(transform.position, ray.point, Color.black);
    }
}
