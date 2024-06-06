using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PlayerStatsMenu : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();

        float r = FindObjectOfType<PlayerStats>().data.playercolor_r;
        float g = FindObjectOfType<PlayerStats>().data.playercolor_g;
        float b = FindObjectOfType<PlayerStats>().data.playercolor_b;

        nameText.text = FindObjectOfType<PlayerStats>().data.name;
        //nameText.color = new Color(r / 255, g / 255, b / 255);
        levelText.text = Mathf.CeilToInt((FindObjectOfType<PlayerStats>().data.level / 100) + 1).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
