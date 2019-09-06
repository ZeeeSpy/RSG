using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowParent : MonoBehaviour
{
    private CameraFollow[] cameralist;

    private void Awake()
    {
        cameralist = new CameraFollow[3];
        int i = 0;
        foreach (Transform child in transform)
        {
            cameralist[i] = child.GetComponent<CameraFollow>();
            i++;
        }

    }

    public void UpdateCameraSmooth(float incsmoothTime, float inclookaheadSmoothtime, bool fix)
    {
        for (int i = 0; i < cameralist.Length; i++)
        {
            cameralist[i].UpdateCameraSmooth(incsmoothTime, inclookaheadSmoothtime, fix);
        }
    }
}
