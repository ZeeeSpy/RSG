/*
 *  Script that is in charge of the enemy AI. including path finding between waypoints, Alert Phase, Shooting phase etc
 *  TODO: add a caution stage to enemy ai inbetween alert and normal
 *  TODO: add flocking behaviour to enemy pathfinding
 */

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
    public Transform returnposition;
    public Transform eyesfront;
    private GlobalPatrolSystem patrolSystem;
    readonly private float patrolspeed = 50f;
    readonly private float alertspeed = 225f;
    private float currentspeed;
    private float nextWaypoitnDistance = 0.1f;
    public bool secondfloor = false;

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
    public bool alert = false;
    private float coneturntime;
    readonly private float alertconeturntime = 100.0f;
    readonly private float normalconeturntime = 1.0f;
    private float lostvisiontime;
    readonly private float resettime = 5f;
    private bool playercurrentlyvisible = false;
    private GlobalAlertScript globalalert;
    public bool calledin = false;

    //Shooting Mode
    private float shootingtimesetup = 0.75f;
    private float shootingtime = 0.75f;
    readonly private float shootingresettime = 0.75f;
    private bool shootingstance = false;

    Transform[] playerhitboxes = new Transform[4];

    public GameObject bulletprefab;
    public bool stunned = false;
    readonly private float bulletSpeed = 3f;

    private int HP = 3;
    private float stuntime = 1.5f;
    private SpriteRenderer thissprite;

    public AudioClip gunsound;
    AudioSource audioSource;

    public bool stationary = false;
    bool dead = false;
    private bool wasblocked = false;

    void Start()
    {
        ////Slow at start but removes dependencies that will be removed by prefabing
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("PlayerBody").GetComponent<Transform>();
        viewconeobject = transform.Find("ViewCone").gameObject;
        viewcone = viewconeobject.GetComponent<Transform>();
        thisviewcone = viewcone.GetComponent<EnemyViewCone>();
        thissprite = transform.Find("EnemySprite").GetComponent<SpriteRenderer>();

        globalalert = (GlobalAlertScript)Object.FindObjectOfType(typeof(GlobalAlertScript));

        //player detection
        Physics2D.queriesStartInColliders = false;

        //patrol setup
        if (!stationary)
        {
            patrolSystem = (GlobalPatrolSystem)Object.FindObjectOfType(typeof(GlobalPatrolSystem));
            patrolpoints = patrolSystem.GetPatrolRoute(routenumber);
            target = patrolpoints[patrolcount];
        } else
        {
            Transform[] patrolpoints = new Transform[2];
            patrolpoints[1] = returnposition;
            patrolpoints[0] = eyesfront;
            patrolcount++;
            target = patrolpoints[patrolcount];
        }


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



        //anti partial obscure
        for (int i = 0; i < playerhitboxes.Length; i++)
        {
            playerhitboxes[i] = GameObject.FindWithTag("PlayerDetectionSquares").transform.GetChild(i).GetComponent<Transform>();
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
        }
        else
        {
            reachedEndofPath = false;
        }

        if (!stunned) //if enemy is stunned they cannot do anything
        {
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
        else //stunned
        {
            stuntime -= Time.deltaTime;
            if (stuntime < 0)
            {
                stunned = false;
                globalalert.GlobalAlertOn();
                stuntime = 3f;
            }
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone()) //if seeker is done
        {
            if (!alert) //and there isn't an alert
            {
                if (reachedEndofPath)
                {
                    if (!stationary)
                    {
                        Cyclepatrol(); //cylce patrol points
                    } else {
                        StationaryCyclepatrol(); //cylce patrol as stationary enemy
                    }
                }
            }

            else //alert
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
            if (wasblocked && calledin) //if they were blocked previously but player moved onto same floor continue as normal
            {
                NormalMode();
                wasblocked = false;
            }
        } else
        {
            //Cannot go to player because they are on a different floor
            if (!calledin)
            {
                globalalert.LostVisual();
                calledin = true;
                wasblocked = true;
            }

            if (calledin && !globalalert.GetGlobalAlert()) 
            {
                NormalMode();
            }

        }
    }

    /*************************
        Move Towards Target
    **************************/
    private void MovetowardsTargetPosition()
    {
        if (!shootingstance) //enemy doesn't move while shooting
        {
            /* stop just before target 0.1 is close enough to "reach destination" and cycle patrol
            but far enough so that the sprite doesn't hit the waypoint */
            if (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = direction * currentspeed * Time.deltaTime;
                rb.AddForce(force);
            }
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

        if (globalalert.GetGlobalAlert() == true)
        {
            AlertMode();
        }

        bool playerseen = ScanForPlayer();
        bool viewconeseen = thisviewcone.isDetected();

        /*************************
         Detect Player
        **************************/

        if (playerseen && viewconeseen) //If vision cone and ray hit player
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
        if (playerseen && newray.distance <= 1.3) //if player isn't obstructed then player is visible
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
                //Debug.Log("Bang!");
                StartCoroutine(BurstFire()); //used to be shooting
                shootingstance = false;
                target = player;
                shootingtime = shootingresettime;
                if (!playercurrentlyvisible)
                {
                    shootingtimesetup = shootingresettime;
                }
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
        if (stationary)
        {
            patrolcount = 0;
            target = eyesfront;
        } else
        {
            target = patrolpoints[patrolcount];
        }
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

    private void StationaryCyclepatrol()
    {
        if (patrolcount == 1)
        {
            patrolcount++;
            target = returnposition;          
        }

        if (patrolcount == 0)
        {
            patrolcount++;
            target = eyesfront;
        }

        if (patrolcount == -1)
        {
            patrolcount++;
        }
    }

    public void gethit(int damage)
    {
        HP = HP - damage;
        stunned = true;
        StartCoroutine(Flash());
        if (HP <= 0)
        {
            DestroySelf();
        }
      
    }

    private void DestroySelf()
    {
        if (dead == false)
        {
            dead = true;
            globalalert.EnemyExit(routenumber);
            Destroy(gameObject);
        }
    }

    IEnumerator Flash()
    {
        for (int n = 0; n < 2; n++)
        {
            thissprite.color = (Color.black);
            yield return new WaitForSeconds(0.1f);
            thissprite.color = (Color.green);
            yield return new WaitForSeconds(0.1f);
        }
    }


    IEnumerator BurstFire()
    {
        audioSource.PlayOneShot(gunsound, 1f);
        GameObject bullet = Instantiate(bulletprefab, transform.position, Quaternion.identity);
        bullet.GetComponent<EBulletScript>().velocity = (player.position - transform.position) * bulletSpeed;
        Destroy(bullet, 1f);
        yield return new WaitForSeconds(0.1f);
        GameObject bullet2 = Instantiate(bulletprefab, transform.position, Quaternion.identity);
        bullet2.GetComponent<EBulletScript>().velocity = (player.position - transform.position) * bulletSpeed;
        Destroy(bullet2, 1f);
        yield return new WaitForSeconds(0.1f);
        GameObject bullet3 = Instantiate(bulletprefab, transform.position, Quaternion.identity);
        bullet3.GetComponent<EBulletScript>().velocity = (player.position - transform.position) * bulletSpeed;
        Destroy(bullet3, 1f);
    }

    public void SetPatrolNumber(int numbertoassign)
    {
        routenumber = numbertoassign;
    }

    public void CheckLightSwitch(Transform lighttocheck)
    {
        if (!alert)
        {
            target = lighttocheck; //auto returns to patrol
            if (stationary)
            {
                patrolcount = -1;
            }
        }
    }

    public void NightView()
    {
        thisviewcone.NightView();
    }

    public void NormalView()
    {
        thisviewcone.NormalView();
    }

    public bool IsSecondFloor()
    {
        return secondfloor;
    }
}