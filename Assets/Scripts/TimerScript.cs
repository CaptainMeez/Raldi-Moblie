using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject timer;

    void Update()
    {
        text.text = Calculations.GetFormattedTime(GameControllerScript.current.playTime);
    }
}