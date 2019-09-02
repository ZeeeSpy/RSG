/*
 * Script used by Destryo Escape and Jet Escape to finish the level.
 * This includes gathering player stats from various other script and calculating a score
 */

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class FinishLevel : MonoBehaviour
{
    private int timeelapsed = 0;
    public GameObject player;
    public GameObject gamesystems;
    public Canvas thiscanvas;

    public Text TotalAlerts;
    public Text EnemiesKilled;
    public Text TotalShots;
    public Text TimeTaken;
    public Text MissionRank;
    public AudioClip missioncompletesound;

    private AudioSource thisaudiosource;

    private GlobalAlertScript globalalrtscript;
    private GlobalPatrolSystem globalpatrolscript;
    private MCMovement playershots;

    private void Start()
    {
        globalalrtscript = (GlobalAlertScript)Object.FindObjectOfType(typeof(GlobalAlertScript));
        globalpatrolscript = (GlobalPatrolSystem)Object.FindObjectOfType(typeof(GlobalPatrolSystem));
        playershots = (MCMovement)Object.FindObjectOfType(typeof(MCMovement));
        thisaudiosource = transform.GetComponent<AudioSource>();
        StartCoroutine(GameTimer());
    }

    public void ShowStats(bool destroy, bool clearance)
    {
        thisaudiosource.PlayOneShot(missioncompletesound);
        thiscanvas.enabled = true;
        StopAllCoroutines();
        string formattedtime = ConvertSecondsToString();
        int alertcount = globalalrtscript.GetAlertCount();
        int deadenemies = globalpatrolscript.GetKilledEnemies();
        int totalshots = playershots.GetShotsFired();

        int score = ((600-timeelapsed)*100)+((1000*(1-(alertcount/10))-(deadenemies*50)));
        //cool magic numbers right?

        TimeTaken.text = formattedtime;
        TotalAlerts.text = alertcount.ToString();
        EnemiesKilled.text = deadenemies.ToString();
        TotalShots.text = totalshots.ToString();
        player.SetActive(false);
        gamesystems.SetActive(false);
        MissionRank.text = score.ToString();
}

    IEnumerator GameTimer()
    {
        while (true)
        {
            timeelapsed++;
            yield return new WaitForSeconds(1);
        }
    }

    private string ConvertSecondsToString()
    {
        int seconds = timeelapsed % 60;
        int minutes = (timeelapsed / 60) % 60;
        int hours = (timeelapsed/3600)%24;

        return (string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds));
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("L1-1");
    }
}
