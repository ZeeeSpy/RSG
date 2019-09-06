using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    private bool paused = false;
    public Canvas pausemenu;
    public CameraFollowParent cameraparent;
    public Slider followsmooth;
    public Slider LookAheadSmooth;
    public Toggle PixelSnap;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("PixelSnap")){
            followsmooth.value = PlayerPrefs.GetFloat("FollowSmooth");
            LookAheadSmooth.value = PlayerPrefs.GetFloat("LookAheadSmooth");
            if (PlayerPrefs.GetInt("PixelSnap") == 1)
            {
                PixelSnap.isOn = true;
            } else
            {
                PixelSnap.isOn = false;
            }

            applyvalues();
        } else //set up player prefs
        {
            resetvalues();
        }
    }

    
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            paused = !paused;
            Debug.Log("Pause Toggle");
            if (paused)
            {
                pausemenu.enabled = true;
                Time.timeScale = 0f;
            } else
            {
                pausemenu.enabled = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void applyvalues()
    {
        cameraparent.UpdateCameraSmooth(followsmooth.value, LookAheadSmooth.value, PixelSnap.isOn);
        SetPref("FollowSmooth", followsmooth.value);
        SetPref("LookAheadSmooth", LookAheadSmooth.value);

        if (PixelSnap.isOn)
        {
            SetPref("PixelSnap", 1);
        } else
        {
            SetPref("PixelSnap", 0);
        }
    }

    public void resetvalues()
    {
        followsmooth.value = 0.5f;
        LookAheadSmooth.value = 0.2f;
        PixelSnap.isOn = true;
        applyvalues();
    }

    private void SetPref(string prefname,float number)
    {
        PlayerPrefs.SetFloat(prefname, number);
    }

    private void SetPref(string prefname,int number)
    {
        PlayerPrefs.SetInt(prefname, number);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
