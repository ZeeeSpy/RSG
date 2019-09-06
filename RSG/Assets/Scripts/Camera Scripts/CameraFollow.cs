/*
 *  Script used for camera follow the player while staying within certain bounds. 
 *  Currently there's a problem with the camera sitting inbetween pixels on non supported resolutions 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    Vector3 velocity = Vector3.zero;

    private float smoothTime = 0.5f;
    private float lookaheadSmoothtime = 0.2f;
    private bool pixelfix = true;
    public float yMinValue;
    public float yMaxValue;
    public float xMinValue;
    public float xMaxValue;
    private readonly float pixelsize = 100;
    private readonly float ypixelsize = 75;
    private readonly float lookaheaddist = 1.1f;


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = player.position + new Vector3(Input.GetAxis("CamHor") * (lookaheaddist), Input.GetAxis("CamVer") * (lookaheaddist), -10f); ;

        targetPos.y = Mathf.Clamp(targetPos.y, yMinValue, yMaxValue);
        targetPos.x = Mathf.Clamp(targetPos.x, xMinValue, xMaxValue);

        if (Input.GetAxis("CamHor") != 0|| Input.GetAxis("CamVer") != 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, lookaheadSmoothtime);
        } else
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }

        /*
         * Art assets are 100pixels per unit. 
         * 
         * Trying to make the camera pixel perfect (Correcting to 0.00) causes aliasing issues.
         * Solution V2:
         * This makes the camera snap to a precision of 0.00 on the x axis.
         * on the y axis it will snap as if the pixel size is 75. (which it isn't) but stops aliasing
         * 
         * This causes camera issues
         * 
         * moving along x,y is okay but moving in 8 directions causes visible jitter.
         * 
         * Posibile solutions:
         * 1) Increase smooth time - This causes camera to "lag" behind
         * 2) Turn off pixel snapped - This causes aliasing issues out the ass. some people don't mind though
         * 2) Have different smooth times for look ahead and normal that can be edited by player - its player problem now
         */

        if (pixelfix)
        {
            transform.position = new Vector3((Mathf.RoundToInt(gameObject.transform.position.x * pixelsize) / pixelsize),
                                            (Mathf.RoundToInt(gameObject.transform.position.y * ypixelsize) / ypixelsize),
                                            (Mathf.RoundToInt(gameObject.transform.position.z * pixelsize) / pixelsize));
        }
    }

    public void UpdateCameraSmooth(float incsmoothTime, float inclookaheadSmoothtime, bool fix)
    {
        smoothTime = incsmoothTime;
        lookaheadSmoothtime = inclookaheadSmoothtime;
        pixelfix = fix;
    }
}
