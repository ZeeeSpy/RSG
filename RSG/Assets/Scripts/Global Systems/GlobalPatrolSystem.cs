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
    public Transform[] GetPatrolRoute(int routenumber) {

        Transform temp = transform.Find("PatrolWayPoints");
        Transform[] patrolpoints = new Transform[temp.childCount];
        for (int i = 0; i < patrolpoints.Length; i++)
        {
            patrolpoints[i] = temp.GetChild(i).GetComponent<Transform>();
        }

        int routestartnumber = 0;
        if (routenumber != 0)
        {
            routestartnumber = (routenumber * 4);
        }
        Transform[] arraytoreturn = new Transform[4]; //Patrols are always 4 long for now
        Array.Copy(patrolpoints, routestartnumber, arraytoreturn, 0, 4);
        return arraytoreturn;

    }
}
