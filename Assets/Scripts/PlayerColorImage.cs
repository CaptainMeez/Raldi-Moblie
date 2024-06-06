using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorImage : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();
    }
    
    private void Update()
    {
        FindObjectOfType<PlayerStats>().TryLoad();
        
        float r = FindObjectOfType<PlayerStats>().data.playercolor_r;
        float g = FindObjectOfType<PlayerStats>().data.playercolor_g;
        float b = FindObjectOfType<PlayerStats>().data.playercolor_b;

        GetComponent<UnityEngine.UI.Image>().color = new Color(r / 255, g / 255, b / 255);
    }
}
