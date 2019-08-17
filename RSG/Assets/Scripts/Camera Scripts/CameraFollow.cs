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
    public float lookaheaddist = 1;


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = player.position + new Vector3(Input.GetAxis("CamHor") * (lookaheaddist), Input.GetAxis("CamVer") * (lookaheaddist), -10f); ;

        targetPos.y = Mathf.Clamp(targetPos.y, yMinValue, yMaxValue);
        targetPos.x = Mathf.Clamp(targetPos.x, xMinValue, xMaxValue);


        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        transform.position = new Vector3((Mathf.RoundToInt(gameObject.transform.position.x * pixelsize) / pixelsize),
                                        (Mathf.RoundToInt(gameObject.transform.position.y * pixelsize) / pixelsize),
                                        (Mathf.RoundToInt(gameObject.transform.position.z * pixelsize) / pixelsize));
    }
}
