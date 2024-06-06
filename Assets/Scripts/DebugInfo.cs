using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DebugInfo : MonoBehaviour
{
    public GameControllerScript gc;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = base.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "";
        text.text += "Debug:";
        text.text += "\nPoopTime = " + gc.player.poopCooldown;
        text.text += "\nHasToPoop = " + gc.player.hasToPoop;
        text.text += "\nDiarrheaPoopMultiplier = " + gc.player.poopMultiplier;
    }
}
