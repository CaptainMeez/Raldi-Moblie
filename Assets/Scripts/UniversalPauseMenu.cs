using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalPauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject loadingScreen;
    private bool paused = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                UnPause();
            }
            else
            {
                menu.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            paused = !paused;
        }

        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                Scroll(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                Scroll(1);
            else if (Input.GetKeyDown(KeyCode.Return))
                Enter(curSelect);
        }
    }

    public void UnPause()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public int curSelect;
    public TMPro.TextMeshProUGUI[] texts;

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

    public void Enter(int index)
    {
        switch (index)
        {
            case 0:
                UnPause();
                break;
            case 1:
                loadingScreen.SetActive(true);
                StartCoroutine(loadingScreen.GetComponent<LoadingScreen>().LoadingLoadSceneInt(SceneManager.GetActiveScene().buildIndex));
                break;
            case 2:
                loadingScreen.SetActive(true);
                StartCoroutine(loadingScreen.GetComponent<LoadingScreen>().LoadingLoadScene("MainMenu"));
                break;
        }
    }
}
