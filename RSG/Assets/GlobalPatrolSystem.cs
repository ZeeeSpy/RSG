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
