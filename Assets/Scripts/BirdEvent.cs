using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class BirdEvent : MonoBehaviour
{
    public TextMeshProUGUI text;
    private string time;
    private string day;
    private string halfofday;
    private string[] months =
    {
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    };
    private string[] correctsuffixes =
    {
        "st",
        "nd",
        "rd",
        "th"
    };
    public void CalculateTime()
    {
        if (DateTime.Now.Hour > 12)
        {
            time = (DateTime.Now.Hour - 12).ToString() + ":";
            halfofday = " PM";
        } else {
            time = (DateTime.Now.Hour).ToString() + ":";
            halfofday = " AM";
        }
        if (DateTime.Now.Minute > 10)
            time = time + (DateTime.Now.Minute).ToString();
        else
            time = time + "0" + (DateTime.Now.Minute).ToString();
        time += halfofday;

        day = months[DateTime.Now.Month - 1].ToUpper() + " " + DateTime.Now.Day.ToString().ToUpper();
        if ((DateTime.Now.Day - 1) % 10 < 4 && Mathf.Floor(DateTime.Now.Day /10) != 1)
        {
            day += correctsuffixes[(DateTime.Now.Day - 1) % 10];
        } else {
            day += correctsuffixes[3];
        }
        text.text = "WHEN IT'S " + day.ToUpper() + " AT " + time.ToUpper() + " AND 14 BIRDS ARE FLYING BY THE WINDOW";
    }


    void Update() //text.text = "WHEN IT'S " + months[DateTime.Now.Month - 1].ToUpper() + " " + DateTime.Now.Day.ToString().ToUpper() + "TH AT " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " AND 14 BIRDS ARE FLYING BY THE WINDOW";
    {
        CalculateTime();
    }
}
