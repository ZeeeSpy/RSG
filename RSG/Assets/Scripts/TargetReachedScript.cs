using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReachedScript : MonoBehaviour
{
    private GameOverScript gameover;

    private void Start()
    {
        gameover = (GameOverScript)Object.FindObjectOfType(typeof(GameOverScript));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            gameover.GameOver();
        }
    }
}
