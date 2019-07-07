using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    //Pathfinding
    private Transform player;
    private Transform target;

    readonly private float patrolspeed = 50f;
    readonly private float alertspeed = 200f;
    private float currentspeed;
    private float nextWaypoitnDistance = 0.1f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    //Patrol points //TODO
    private int patrolcount = 0;
    public Transform patrol1;
    public Transform patrol2;
    public Transform patrol3;
    private Transform[] patrolpoints = new Transform[3];

    //Player Detection
    private Transform viewcone;
    private EnemyViewCone thisviewcone;
    Vector2 lookahead;
    

    //Alert Mode
    private bool alert = false;
    private float coneturntime;
    readonly private float alertconeturntime = 25.0f;
    readonly private float normalconeturntime = 1.0f;
    private float lostvisiontime;
    readonly private float resettime = 10f;
    private bool playercurrentlyvisible = false;
    private GlobalMusicScript musicscript;

    void Start()
    {
        ////Slow at start but removes dependencies that will be removed by prefabing
        player = GameObject.FindWithTag("PlayerBody").GetComponent<Transform>();
        viewcone = transform.Find("ViewCone").gameObject.GetComponent<Transform>();
        thisviewcone = viewcone.GetComponent<EnemyViewCone>();
        musicscript = (GlobalMusicScript)Object.FindObjectOfType(typeof(GlobalMusicScript));

        //player detection
        Physics2D.queriesStartInColliders = false;
        // foo = Object.FindObjectWithTag("Bar");

        //patrol setup
        patrolpoints[0] = patrol1;
        patrolpoints[1] = patrol2;
        patrolpoints[2] = patrol3;
        target = patrolpoints[patrolcount];

        //pathfinding setup
        currentspeed = patrolspeed;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        lookahead = Vector2.zero;
        InvokeRepeating("UpdatePath", 0f, 0.1f);

        //alert setup
        lostvisiontime = resettime;
        coneturntime = normalconeturntime;
    }


    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (!alert)
            {
                if (reachedEndofPath)
                {
                    Cyclepatrol();
                }
            } else //alert
            {
                //on alert set player as target and move fast
                target = player.transform;
                currentspeed = alertspeed;
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


    /*************************
        Main Update Loop
    **************************/
    void FixedUpdate()
    {
       //If no path do nothing check
       if (path == null)
        {
            return;
        }

       //if end of path update bools
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

    /*************************
        Move Towards Target
    **************************/
    private void MovetowardsTargetPosition()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * currentspeed * Time.deltaTime;

        rb.AddForce(force);
    }

    /*************************
        Face cone to target
    **************************/
    private void FaceConeToTarget()
    {
        try
        {
            //look ahead 4 waypoints, calculate angle of target relative to enemy, lerp between current cone position and look ahead position
            lookahead = ((Vector2)path.vectorPath[currentWaypoint + 5] - rb.position).normalized;
            float rotationZ = Mathf.Atan2(lookahead.y * 10, lookahead.x * 10) * Mathf.Rad2Deg;
            viewcone.rotation = Quaternion.Slerp(viewcone.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ + 90), coneturntime * Time.deltaTime);
        }
        catch
        {
            //exepcted behvaiour do nothing.when within 4 waypoints (0.24 in game units, the enemy will not look elsewhere)
        }
    }

    /*************************
        Detect Player/Alert Stuff
    **************************/
    private void CheckIfPlayerIsDetected()
    {
        RaycastHit2D playerray = Physics2D.Linecast(this.transform.position, player.position); 

        //see line for debugging
        Debug.DrawLine(transform.position, playerray.point, Color.white);

        if (playerray.collider.transform.tag == "PlayerBody" && thisviewcone.isDetected()) //If vision cone and ray hit player
        {
            lostvisiontime = resettime;
            playercurrentlyvisible = true;
            alert = true;
            coneturntime = alertconeturntime;
            musicscript.startalertmusic();
        } else //Player currently not visible
        {
            playercurrentlyvisible = false;
        }

        if (playerray.collider.transform.tag == "PlayerBody" && playerray.distance <= 1.5) //if player isn't obstructed then player is visible
        {
            playercurrentlyvisible = true;
        }

        if (alert && !playercurrentlyvisible)   //if there's an alert and player is not visible
        {
            lostvisiontime -= Time.deltaTime;
            Debug.Log(lostvisiontime);
            if (lostvisiontime < 0)     //if there's an alert and player is not visible for a total of lostvisiontime 
            {
                alert = false;
                musicscript.startnormalmusic();
                coneturntime = normalconeturntime;
                //add behavious of ai when lost sight of player
                //currently ai goes back to normal patrol route
                //refactor so that alert and normal are functions that can be called to clean up code 
                target = patrolpoints[patrolcount];
                currentspeed = patrolspeed;
            }
        }
    } 

    private void Cyclepatrol()
    {
        patrolcount++;
        if (patrolcount == patrolpoints.Length)
        {
            patrolcount = 0;
        }
        target = patrolpoints[patrolcount];
    }
}
