﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FightSelectScript : MonoBehaviour
{
    public SelectHud parent;
    public int selectedButton;
    public bool canInteract = false;
    public TextMeshProUGUI[] buttons;
    public string[] attacks = {"Whack", "Pew Pew"};
    public GameObject attackUI;
    
    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Select(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                Select(1);
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
                EnterSubMenu();
        }
    }

    private void Select(int change)
    {
        parent.auSource.PlayOneShot(parent.impact);

        selectedButton += change;

        if (selectedButton > buttons.Length - 1)
            selectedButton = 0;
        else if (selectedButton < 0)
            selectedButton = buttons.Length - 1;

        for(int i = 0; i < buttons.Length; i++)
        {
            if (selectedButton == i)
            {
                buttons[i].color = Color.white;
            }
            else
            {
                buttons[i].color = Color.gray;
            }
        }
    }
    
    public void EnterSubMenu()
    {
        canInteract = false;
        parent.auSource.PlayOneShot(parent.enter);

        if (selectedButton == 0)
        {
            parent.inSubMenu = false;
            attackUI.SetActive(true);
            attackUI.GetComponent<ThrowAttackScript>().canInteract = true;
            attackUI.GetComponent<ThrowAttackScript>().Reset(gameObject);
        }
    }
}
