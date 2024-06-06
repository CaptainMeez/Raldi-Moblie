using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class RecordText : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();

        text = GetComponent<TextMeshProUGUI>();
        
        if (FindObjectOfType<PlayerStats>().data.storyModeWon)
        {
            float minutes = (Mathf.Floor(FindObjectOfType<PlayerStats>().data.recordTime/60));
            float seconds = (Mathf.Floor(FindObjectOfType<PlayerStats>().data.recordTime % 60));

            if (seconds < 10)
                text.text = "Personal Best: " + minutes + ":0" + seconds;
            else
                text.text = "Personal Best: " + minutes + ":" + seconds;
        }
        else
        {
            if (FindObjectOfType<PlayerStats>().data.recordNormalExits != 0)
                text.text = "Personal Best: " + FindObjectOfType<PlayerStats>().data.recordNormalNotebooks + " Notebooks";
            else 
                text.text = "Personal Best: " + FindObjectOfType<PlayerStats>().data.recordNormalNotebooks + " Notebooks, " + FindObjectOfType<PlayerStats>().data.recordNormalExits + " Exits";
        }
    }
}
