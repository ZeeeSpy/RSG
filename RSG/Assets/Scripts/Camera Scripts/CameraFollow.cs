using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.1f;
    private float yVal = -1.8f;
    private float xMinValue = -3.26f;
    private float xMaxValue = -0.08f;
    private float pixelsize = 600f;


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = player.position;

        targetPos.y = yVal;
        targetPos.x = Mathf.Clamp(player.position.x, xMinValue, xMaxValue);

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        transform.position = new Vector3((Mathf.Round(gameObject.transform.position.x * pixelsize) / pixelsize),
                                        (Mathf.Round(gameObject.transform.position.y * pixelsize) / pixelsize),
                                        (Mathf.Round(gameObject.transform.position.z * pixelsize) / pixelsize));
    }
}
