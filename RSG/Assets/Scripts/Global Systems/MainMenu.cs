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

    public void PlayGame(string levelname)
    {
        SceneManager.LoadScene(levelname);
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
