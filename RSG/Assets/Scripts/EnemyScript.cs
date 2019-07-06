using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    //Pathfinding
    public Transform player;
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


    //Patrol points
    private int patrolcount = 0;
    public Transform patrol1;
    public Transform patrol2;
    public Transform patrol3;
    private Transform[] patrolpoints = new Transform[3];

    void Start()
    {
        //patrol setup
        patrolpoints[0] = patrol1;
        patrolpoints[1] = patrol2;
        patrolpoints[2] = patrol3;
        target = patrolpoints[patrolcount];
        //pathfinding setup
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        lookahead = Vector2.zero;
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (reachedEndofPath)
            {
                cyclepatrol();
            }
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
            viewcone.rotation = Quaternion.Slerp(viewcone.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ + 90), 5f*Time.deltaTime);
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


    private void cyclepatrol()
    {
        patrolcount++;
        if (patrolcount == 3)
        {
            patrolcount = 0;
        }
        target = patrolpoints[patrolcount];
    }
}
