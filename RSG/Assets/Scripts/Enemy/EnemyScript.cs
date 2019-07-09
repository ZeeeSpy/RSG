using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    public int routenumber = 0;

    //Pathfinding
    private Transform player;
    private Transform target;
    private GlobalPatrolSystem patrolSystem;
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
    private Transform[] patrolpoints;

    //Player Detection
    private Transform viewcone;
    private EnemyViewCone thisviewcone;
    private GameObject viewconeobject;
    Vector2 lookahead;
    

    //Alert Mode
    private bool alert = false;
    private float coneturntime;
    readonly private float alertconeturntime = 25.0f;
    readonly private float normalconeturntime = 1.0f;
    private float lostvisiontime;
    readonly private float resettime = 10f;
    private bool playercurrentlyvisible = false;
    private GlobalAlertScript globalalert;
    private bool calledin = false;

    //Shooting Mode
    private float shootingtimesetup = 1f;
    private float shootingtime = 1f;
    readonly private float shootingresettime = 1f;
    private bool shootingstance = false;
    private HeadLineScript headlinescript;


    Transform[] playerhitboxes  = new Transform[4];


    void Start()
    {
        ////Slow at start but removes dependencies that will be removed by prefabing
        player = GameObject.FindWithTag("PlayerBody").GetComponent<Transform>();
        viewconeobject = transform.Find("ViewCone").gameObject;
        viewcone = viewconeobject.GetComponent<Transform>();
        thisviewcone = viewcone.GetComponent<EnemyViewCone>();
        
        globalalert = (GlobalAlertScript)Object.FindObjectOfType(typeof(GlobalAlertScript));

        //player detection
        Physics2D.queriesStartInColliders = false;

        //patrol setup
        patrolSystem = (GlobalPatrolSystem)Object.FindObjectOfType(typeof(GlobalPatrolSystem));
        patrolpoints = patrolSystem.GetPatrolRoute(routenumber);
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
        globalalert.EnemyEnter();

        //shooting
        headlinescript = transform.Find("HeadLine").gameObject.GetComponent<HeadLineScript>();


        //anti partial obscure
        for (int i = 0; i < playerhitboxes.Length; i++)
        {
            playerhitboxes[i] = GameObject.FindWithTag("PlayerDetectionSquares").transform.GetChild(i).GetComponent<Transform>();
        }
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
        if (!shootingstance) //enemy doesn't move while shooting
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * currentspeed * Time.deltaTime;

            rb.AddForce(force);
        }
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
        //Was here
        //
        //RaycastHit2D playerray = Physics2D.Linecast(this.transform.position, player.position); 
        //

        //see line for debugging
        //Debug.DrawLine(transform.position, playerray.point, Color.white);


        if (globalalert.GetGlobalAlert() == true)
        {
            AlertMode();
        }


        /*************************
         Detect Player
        **************************/

            if (ScanForPlayer() && thisviewcone.isDetected()) //If vision cone and ray hit player
            {
                lostvisiontime = resettime;
                playercurrentlyvisible = true;
                AlertMode();
                globalalert.GlobalAlertOn();
                if (calledin)
                {
                    globalalert.GotVisual();
                    calledin = false;
                }
            }
            else //Player currently not visible
            {
                playercurrentlyvisible = false;
            }


            RaycastHit2D newray = Physics2D.Linecast(this.transform.position, player.position);
            if (ScanForPlayer() && newray.distance <= 1.3) //if player isn't obstructed then player is visible
            {
                playercurrentlyvisible = true;
            }


        /*************************
         Shooting Stance
        **************************/

        if (playercurrentlyvisible && alert && !shootingstance)
        {
            shootingtimesetup -= Time.deltaTime;
            if (shootingtimesetup < 0)
            {
                shootingstance = true;
                target = this.transform;
            }
        } 

        if (shootingstance)
        {
            //Debug.Log("ShootingStance");   
            shootingtime -= Time.deltaTime;
            if (shootingtime < 0)
            {
                Debug.Log("Bang!");
                if (headlinescript.Hits(ScanForPlayer()))
                {
                    Debug.Log("PlayerHit");
                    //deal damage to player via GameMaster
                }
                shootingstance = false;
                target = player;
                shootingtime = shootingresettime;
                shootingtimesetup = shootingresettime;
            }
        }


        /*************************
         Unsee Player
        **************************/

        if (alert && !playercurrentlyvisible)   //if there's an alert and player is not visible
        {
            lostvisiontime -= Time.deltaTime;

            //Lost vision timer for debugging
            // Debug.Log(lostvisiontime);
            if (lostvisiontime < 0)     //if there's an alert and player is not visible for a total of lostvisiontime 
            {
                if (!calledin) //Tell GameMaster that player hasn't been visible for lostvisiontime
                {
                    globalalert.LostVisual();
                    calledin = true;
                } 

                if (!globalalert.GetGlobalAlert()) //Check if Gamemaster has ceased the global alert meaning all enemies have lost contact for minimum of lostvisiontime
                {
                    NormalMode();
                }

                //add behavious of ai when lost sight of player
                //currently ai goes back to normal patrol route
            }
        }


    }

    private bool ScanForPlayer()
    {
        bool detected = false;
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D playerray = Physics2D.Linecast(transform.position, playerhitboxes[i].position);
            //Debug.DrawLine(transform.position, playerray.point, Color.black);
            if (playerray)
            {
                if (playerray.collider.transform.tag == "PlayerBody")
                {
                    detected = true;
                }
            }
            else return detected;
            
        }
        return detected;
    }

    private void AlertMode()
    {
        alert = true;
        coneturntime = alertconeturntime;
    }

    private void NormalMode()
    {
        alert = false;
        coneturntime = normalconeturntime;
        target = patrolpoints[patrolcount];
        currentspeed = patrolspeed;
        lostvisiontime = resettime;
        calledin = false;
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