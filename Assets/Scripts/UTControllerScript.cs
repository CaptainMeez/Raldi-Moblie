using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UTControllerScript : MonoBehaviour
{
    public int lv = 1;
    public int hp = 20;
    public bool krEnabled = false;
    public string humanName = "player";
    public TextMeshProUGUI humanNameAndLV;
    public TextMeshProUGUI hpamounttext;
    public bool playerTurn = true;
    public int playerButtonSelected = 0;
    public int kr = 0;
    public int maxHP = 20;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hp >= 10)
            hpamounttext.text = hp + " / " + maxHP;
        else
            hpamounttext.text = "0" + hp + " / " + maxHP;
        if (Input.GetKeyDown(KeyCode.A))
        {
            lv--;
            UpdateStats();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lv++;
            UpdateStats();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SelectChange(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SelectChange(1);
    }

    public void UpdateStats()
    {
        UpdateStatsNoHPReset();
        hp = maxHP;
    }
    
    public void UpdateStatsNoHPReset()
    {
        maxHP = Mathf.FloorToInt((lv*4)+16+(Mathf.Floor(lv/20)*3));
        humanNameAndLV.text = humanName + " LV " + lv;
    }

    public void SelectChange(int change)
    {
        playerButtonSelected += change;
        if (playerButtonSelected > 3)
        {
            playerButtonSelected = 0;
        } else if (playerButtonSelected < 0)
        {
            playerButtonSelected = 3;
        }
    }
}
