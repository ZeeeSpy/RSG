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
    public SceneAsset targetscene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            SceneManager.LoadScene(targetscene.name, LoadSceneMode.Single);
        }
    }
}
