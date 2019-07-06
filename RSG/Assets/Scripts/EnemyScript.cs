using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    //Pathfinding
    public Transform player;
    public Transform target;

    private float speed = 25f;
    //private float alertspeed = 100f;
    private float nextWaypoitnDistance = 4f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    //Patrol points
    private int patrolcount = 0;
    public Transform patrol1;
    public Transform patrol2;
    public Transform patrol3;
    private Transform[] patrolpoints = new Transform[3];

    //Player Detection
    public Transform viewcone;
    Vector2 lookahead;
    public float raydistance = 0.5f;
    public EnemyViewCone thisviewcone;

    void Start()
    {
        //player detection
        Physics2D.queriesStartInColliders = false;

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
                //Change to make alert phase work
                Cyclepatrol();
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
       //If no path do nothing check
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

        MovetowardsTargetPosition();
        FaceConeToTarget();
        CheckIfPlayerIsDetected();

        //continue to next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypoitnDistance)
        {
            currentWaypoint++;
        }
    }

    private void MovetowardsTargetPosition()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
    }

    private void FaceConeToTarget()
    {
        try
        {
            //look ahead 4 waypoints, calculate angle of target relative to enemy, lerp between current cone position and look ahead position
            lookahead = ((Vector2)path.vectorPath[currentWaypoint + 5] - rb.position).normalized;
            float rotationZ = Mathf.Atan2(lookahead.y * 10, lookahead.x * 10) * Mathf.Rad2Deg;
            viewcone.rotation = Quaternion.Slerp(viewcone.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ + 90), 5f * Time.deltaTime);
        }
        catch
        {
            //exepcted behvaiour do nothing.when within 4 waypoints (0.24 in game units, the enemy will not look elsewhere)
        }
    }

    private void CheckIfPlayerIsDetected()
    {
        RaycastHit2D playerray = Physics2D.Linecast(this.transform.position, player.position);

        //see line for debugging
        //Debug.DrawLine(transform.position, playerray.point, Color.white);

        if (playerray.collider.transform.tag == "PlayerBody" && thisviewcone.isDetected())
        {
            Debug.Log("PlayerSpotted");
            //switch to alert mode and follow player
        }
    }

    private void Cyclepatrol()
    {
        patrolcount++;
        if (patrolcount == 3)
        {
            patrolcount = 0;
        }
        target = patrolpoints[patrolcount];
    }
}
