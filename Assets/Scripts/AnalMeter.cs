using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class AnalMeter : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Update()
    {
        if (GameControllerScript.current.player.hasToPoop)
            text.text = "NOW.";
        else
            text.text = Mathf.RoundToInt(GameControllerScript.current.player.poopCooldown).ToString() + " seconds.";
    }
}