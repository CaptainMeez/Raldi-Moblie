using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Rewired;

public class PauseMenuScript : MonoBehaviour
{
    public int curSelect;
    public TMPro.TextMeshProUGUI[] texts;

    private Player input;

    private void Start()
    {
        input = ReInput.players.GetPlayer(0);
    }

    private void OnEnable()
    {
        int index = 0;

        foreach(TMPro.TextMeshProUGUI text in texts)
        {
            if (index == curSelect)
                text.color = Color.white;
            else
                text.color = Color.grey;

            index++;
        }
    }

    public void Scroll(int change)
    {
        curSelect += change;

        if (curSelect < 0)
            curSelect = texts.Length - 1;
        else if (curSelect > texts.Length - 1)
            curSelect = 0;

        int index = 0;

        foreach(TMPro.TextMeshProUGUI text in texts)
        {
            if (index == curSelect)
                text.color = Color.white;
            else
                text.color = Color.grey;

            index++;
        }
    }

    public void Set(int inset)
    {
        curSelect = inset;

        int index = 0;

        foreach(TMPro.TextMeshProUGUI text in texts)
        {
            if (index == curSelect)
                text.color = Color.white;
            else
                text.color = Color.grey;

            index++;
        }
    }

    public void Enter()
    {
        switch (curSelect)
        {
            case 0:
                gc.UnpauseGame();
                break;
            case 1:
                gc.Reset();
                break;
            case 2:
                gc.ExitGame();
                break;
        }
    }

    private void Update()
    {
        if (input.GetButtonDown("MenuUp"))
            Scroll(-1);
        else if (input.GetButtonDown("MenuDown"))
            Scroll(1);
        else if (input.GetButtonDown("Skip") || input.GetButtonDown("Jump")) // USING JUMP BUTTON BECAUSE I'M LAZY LOLOLOL
            Enter();
    }

    public GameControllerScript gc;
}