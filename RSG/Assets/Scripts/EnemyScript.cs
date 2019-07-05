using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    //Pathfinding
    public Transform target;
    public float speed = 75f;
    public float nextWaypoitnDistance = 1f;
    Vector2 lookahead;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    public Transform viewcone;

    void Start()
    {
        //pathfinding setup
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .1f);
        lookahead = Vector2.zero;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if (path == null)
        {
            return;
        }

       if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        } else
        {
            reachedEndofPath = false;
        }

       Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
       Vector2 force = direction * speed * Time.deltaTime;
       
       rb.AddForce(force);

        try {
            //look ahead 4 waypoints, calculate angle of target relative to enemy, lerp between current cone position and look ahead position
            lookahead = ((Vector2)path.vectorPath[currentWaypoint + 5] - rb.position).normalized;
            float rotationZ = Mathf.Atan2(lookahead.y * 10, lookahead.x * 10) * Mathf.Rad2Deg;
            viewcone.rotation = Quaternion.Lerp(viewcone.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ + 90), 5f*Time.deltaTime);
        } catch
        {
            //exepcted behvaiour do nothing.when within 4 waypoints (0.24 in game units, the enemy will not look elsewhere)
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypoitnDistance)
        {
            currentWaypoint++;
        }
    }

}
