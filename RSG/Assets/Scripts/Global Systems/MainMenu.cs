/*
 *  Script used to navigate the main menu
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource menubuttonsound;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Add loading screen
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void buttonsound()
    {
        menubuttonsound.Play();
    }
}
