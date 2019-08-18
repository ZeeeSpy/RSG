/*
 *  Generic Scene Changer, used in tutorials and debugging
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.SceneManagement;

public class TargetReachedScript : MonoBehaviour
{
    public string targetscene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            SceneManager.LoadScene(targetscene, LoadSceneMode.Single);
        }
    }
}
