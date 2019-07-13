using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMineScript : MonoBehaviour
{
    public CircleCollider2D trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyScript>().gethit(5); //Need to change to non lethal
            Destroy(gameObject);
        }
    }
}
