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
        if (collision.gameObject.tag == "PlayerBody")
        {
            detected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBody")
        {
            detected = false;
        }
    }

    public bool isDetected()
    {
        return detected;
    }

}
