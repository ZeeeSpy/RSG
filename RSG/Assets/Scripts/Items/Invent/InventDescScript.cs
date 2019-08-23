/*
 *  Script used to update the visuals on the inventory screen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventDescScript : MonoBehaviour
{
    public Text ItemText;
    public Text Stats;
    public Text PlayerStats;
    public Image ItemImage;
    private int playtime = 0;


    void Start()
    {
        StartCoroutine(TimePassed());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(string itemname, string itemdesc, Sprite itemimage)
    {
        ItemImage.sprite = itemimage;
        ItemText.text = itemdesc;
        Stats.text = itemname;
    }

    public void UpdatePlayerStats(){

    }

    IEnumerator TimePassed()
    {
        while (true)
        {
            playtime++;
            yield return new WaitForSeconds(1);
        }
    }
}
