using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewCone : MonoBehaviour
{
    bool detected; 
    void Start()
    {
        detected = false;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            detected = true;
            Debug.Log("player in cone");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            detected = false;
            Debug.Log("player out of cone");
        }
    }

    public bool isDetected()
    {
        return detected;
    }

}
