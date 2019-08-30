/*
 *  Script used to derive the patrol routes for each enemy. Currently the patrol routes are empty game objects
 *  this iterates through them and gives a slice to each enemy on start when called. Each enemy has a route ID
 *  each route is 4 nodes that loop. 
 *  
 *  so a call of 1 would give nodes 0,1,2,3. a call of 2 would give 4,5,6,7 and so on
 *  
 *  This is expensive, it cannot be run once on start then called by the enemy objects, has to be run once for each call
 *  this is because Unity nulls out the array values at some point after start and before frame 1 where the value is needed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalPatrolSystem : MonoBehaviour
{
    private int numberofpatrols; //max number of possible patrols
    private Transform[] patrolroutemaster; //holds all route nodes
    private bool[] IsGuardAlive; //holds the "aliveness" of each guard
    private int deadguards = 0;
    public GameObject Enemy;
    public GameObject SpawnPoint;
    public GameObject SecondarySpawnPoint;
    private int KilledEnemies = 0;

    private void Awake() //Awake is before start
    {
        Transform temp = transform.Find("PatrolWayPoints");
        Transform[] patrolpoints = new Transform[temp.childCount];
        for (int i = 0; i < patrolpoints.Length; i++)
        {
            patrolpoints[i] = temp.GetChild(i).GetComponent<Transform>();
        }
        patrolroutemaster = (Transform[])patrolpoints.Clone();
        IsGuardAlive = new bool[(patrolpoints.Length / 4)]; //assume all patrols are filled from start
        for (int i = 0; i < IsGuardAlive.Length; i++)
        {
            IsGuardAlive[i] = true; //assumed all patrols are filled and thus all enemies are alive on start
        }
    }

    public Transform[] GetPatrolRoute(int routenumber)
    {
        int routestartnumber = 0;
        if (routenumber != 0)
        {
            routestartnumber = (routenumber * 4);
        }
        Transform[] arraytoreturn = new Transform[4]; //Patrols are always 4 long for now
        Array.Copy(patrolroutemaster, routestartnumber, arraytoreturn, 0, 4);
        return arraytoreturn;
    }

    public void DeadGaurdPatrolRoute(int routenumber)
    {
        KilledEnemies++;
        if (!(routenumber == -1))
        {
            IsGuardAlive[routenumber] = false;
            Debug.Log("RouteNumber: " + routenumber + " needs to be filled");
            deadguards++;
        } else
        {
            Debug.Log("Stationary Guard Dead");
        }
    }

    public void Reenforce()
    {
        if (deadguards == 0)
        {
            return;
        }

        int numberofdead = 0;
        for (int i = 0; i < IsGuardAlive.Length; i++)
        {
            if (IsGuardAlive[i] == false)
            {
                StartCoroutine(SpawnReenfocement(i, numberofdead));
                numberofdead++;
            }
        }
    }

    IEnumerator SpawnReenfocement(int patrolnumber, int order)
    {
        yield return new WaitForSeconds(order * 5f);
        GameObject enemy = Instantiate(Enemy, SpawnPoint.transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyScript>().SetPatrolNumber(patrolnumber);
        if (enemy.GetComponent<EnemyScript>().IsSecondFloor())
        {
            enemy.transform.position = SecondarySpawnPoint.transform.position;
        }
        IsGuardAlive[patrolnumber] = true;
    }

    public int GetKilledEnemies()
    {
        return KilledEnemies;
    }
}
