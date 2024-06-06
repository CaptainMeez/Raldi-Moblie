using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeastStupidText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    float timefunny = 18000;
    
    
    void Update()
    {
        timefunny -= Time.deltaTime;

        if (timefunny > 60)
            text.text = "Last to leave the circle wins a free item!\n" + Mathf.Floor(timefunny/3600) + " hours, " +  Mathf.Floor(timefunny/60) % 60 + " minutes, and " + Mathf.Floor(timefunny % 60) + " seconds remain!";
        else
            text.text = "Last to leave the circle wins a free item!\n" + Mathf.RoundToInt(timefunny).ToString() + " seconds remain!";
    }
}
