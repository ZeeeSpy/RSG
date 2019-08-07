using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMineScript : MonoBehaviour
{
    public CircleCollider2D trigger;
    public GameObject exposionprefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject thisexplosion = Instantiate(exposionprefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
