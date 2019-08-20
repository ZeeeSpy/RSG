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

    private float smoothTime = 0.2f;
    public float yMinValue;
    public float yMaxValue;
    public float xMinValue;
    public float xMaxValue;
    private float pixelsize = 100;
    private float ypixelsize = 75;
    private float lookaheaddist = 1.1f;


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = player.position + new Vector3(Input.GetAxis("CamHor") * (lookaheaddist), Input.GetAxis("CamVer") * (lookaheaddist), -10f); ;

        targetPos.y = Mathf.Clamp(targetPos.y, yMinValue, yMaxValue);
        targetPos.x = Mathf.Clamp(targetPos.x, xMinValue, xMaxValue);


        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        /*
         * Art assets are 100pixels per unit. 
         * 
         * Trying to make the camera pixel perfect (Correcting to 0.00) causes aliasing issues.
         * Solution V2:
         * This makes the camera snap to a precision of 0.00 on the x axis.
         * on the y axis it will snap as if the pixel size is 75. (which it isn't) but stops aliasing
         */
        transform.position = new Vector3((Mathf.RoundToInt(gameObject.transform.position.x * pixelsize) / pixelsize), 
                                        (Mathf.RoundToInt(gameObject.transform.position.y * ypixelsize) / ypixelsize),
                                        (Mathf.RoundToInt(gameObject.transform.position.z * pixelsize) / pixelsize));
    }
}
