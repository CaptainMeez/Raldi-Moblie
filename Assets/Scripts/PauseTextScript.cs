using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PauseTextScript : MonoBehaviour
{
    GameControllerScript gc;
    public TextMeshProUGUI text;
    public string mode = "Unknown";

    // Start is called before the first frame update
    void Start()
    {
        gc = GameControllerScript.current;

        switch(gc.mode)
        {
            case "story":
                mode = "Story Mode";
                break;
            case "endless":
                mode = "Endless";
                break;
            case "hard":
                mode = "Story Mode (HARD MODE)";
                break;
            default:
                mode = "Undefined";
                break;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        string nb = gc.notebooks + "/" + gc.notebooksToCollect + " Notebooks Collected \n";

        if (PlayerPrefs.GetFloat("blind") == 1)
            nb = "";

        text.text = mode + "\n" + nb + SecondsToTime(gc.playTime) + " Elapsed";
    }

    public static string SecondsToTime(float time)
    {
        float minutes = Mathf.Floor(time / 60);
     	float seconds = time % 60;
        
        string formatMinutes = minutes.ToString();

        string formatSeconds = System.Math.Round(seconds).ToString();
        if (formatSeconds.ToCharArray().Length == 1) formatSeconds = "0" + System.Math.Round(seconds).ToString();

        return formatMinutes + ":" + formatSeconds;
    }
}