using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    //Pathfinding
    public Transform target;
    public float speed = 200f;
    public float nextWaypoitnDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        //pathfinding setup
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
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

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypoitnDistance)
        {
            currentWaypoint++;
        }
    }

}
