using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPeter : MonoBehaviour
{
    public MainMenuScript mainMenu;
    float delay = 0f;
    public int clickTimes;
    bool startedClicking = false;
    public bool canClick = true;

    public void Click()
    {
        if (canClick)
        {
            clickTimes++;
            delay = 0.5f;
            startedClicking = true;

            if (clickTimes == 10)
            {
                mainMenu.ActivatePeter();
                canClick = false;
            }
        }
    }

    void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
        }
        else if (delay >= 0f && startedClicking)
        {
            startedClicking = false;
            clickTimes = 0;
        }
    }
}
