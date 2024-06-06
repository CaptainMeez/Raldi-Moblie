using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class walterMethStationScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int level = 1;
    public TextMeshPro compoopertext; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        compoopertext.text = "Meth " + Calculations.ToRomanNumeral(level) + " (" + Calculations.GetFormattedTime(GameControllerScript.current.methTimer) + ")\n$" + "Upgrade for:\n" + GameControllerScript.current.methPrice;
    }

    public void Purchase()
    {
        level++;
    }
}
