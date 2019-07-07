using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraToggle : MonoBehaviour
{

    public Camera previouscam; //camera from previous room
    public Camera targetcam; //camera in target room

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //if collision happens with the player switch cameras, two colliders needed one for each way
        {
            previouscam.enabled = false;
            targetcam.enabled = true;
        } 
    }
}
