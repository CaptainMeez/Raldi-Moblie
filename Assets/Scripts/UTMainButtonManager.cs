using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UTMainButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public UTControllerScript uc;
    public Sprite[] fightImages = new Sprite[2];
    public Sprite[] actImages = new Sprite[2];
    public Sprite[] itemImages = new Sprite[2];
    public Sprite[] mercyImages = new Sprite[2];
    public Image[] buttons = new Image[4];

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (uc.playerTurn)
        {
            buttons[0].sprite = fightImages[Convert.ToInt32(uc.playerButtonSelected == 0)];
            buttons[1].sprite = actImages[Convert.ToInt32(uc.playerButtonSelected == 1)];
            buttons[2].sprite = itemImages[Convert.ToInt32(uc.playerButtonSelected == 2)];
            buttons[3].sprite = mercyImages[Convert.ToInt32(uc.playerButtonSelected == 3)];
        }
    }
}
