using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveFileScreen : MonoBehaviour
{
    public SaveFileSelectionPanel[] panels;

    public PlayerStats playerStats;
    public GameObject selectScreen;
    public GameObject newFileScreen;
    public GameObject loadingScreen; 
    public Sprite neilbackground;
    public Image background;
    public ColorPicker picker;

    public void Start()
    {
        if (PlayerPrefs.GetFloat("NeilScarySave") == 1)
        {
            music[0].gameObject.SetActive(false);
            music[1].gameObject.SetActive(false);

            background.sprite = neilbackground;
        }
    }

    public void OpenSaveMenu(int saveid)
    {
        foreach (SaveFileSelectionPanel panel in panels)
        {
            if (saveid == panel.saveFileNumber)
            {
                if (panel.fileExists)
                    StartCoroutine(exitState(panel));
                else
                {
                    if (PlayerPrefs.GetFloat("PreventNewFiles") == 1)
                    {
                        SceneManager.LoadScene("NeilNewSave");
                    }
                    else
                    {
                        newFileScreen.SetActive(true);
                        selectScreen.SetActive(false);

                        inEditMode = true;

                        SetDefaultSaveValues();

                        picker.OnColorReset();
                     
                        PlayerPrefs.SetInt("CurrentSave", saveid);
                    }
                }
            }
        }
    }

    public void SetDefaultSaveValues()
    {
        playerStats.data.name = "Player";
        playerStats.data.beatHardMode = false;
        playerStats.data.recordScore = 0;
        playerStats.data.foundObjectasks = false;
        playerStats.data.windowIsBroken = false;
        playerStats.data.interactedWithNeil = false;
        playerStats.data.highBooks = 0;
        playerStats.data.recordNormalNotebooks = 0;
        playerStats.data.recordNormalExits = 0;
        playerStats.data.recordTime = 9999;
        playerStats.data.objectasks = new int[8]{0, 0, 0, 0, 0, 0, 0, 0};
        playerStats.data.theEnd = false;
        playerStats.data.storyModeWon = false;
        playerStats.data.cowMode = false;
        playerStats.data.ishaanTimeWahoo = false;
        playerStats.data.ishaanModeUnlocked = false;
        playerStats.data.ishaanMenu = false;
        playerStats.data.canTalkIshaan = false;
        playerStats.data.playercolor_r = 124;
        playerStats.data.playercolor_g = 217;
        playerStats.data.playercolor_b = 150;
        playerStats.data.level = 1;
        playerStats.data.headCosmetic = 0;
        playerStats.data.bodyCosmetic = 0;
        playerStats.data.backCosmetic = 0;
    }

    IEnumerator exitState(SaveFileSelectionPanel panel)
    {
        PlayerPrefs.SetInt("CurrentSave", panel.saveFileNumber);
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("MainMenu");
    }

    public AudioSource[] music;
    public bool inEditMode = false;

    void Update()
    {
        if (!inEditMode)
        {
            if (music[0].volume < 1f)
                music[0].volume += Time.deltaTime / 2;
            if (music[1].volume > 0f)
                music[1].volume -= Time.deltaTime / 2;
        }
        else
        {
            if (music[1].volume < 1f)
                music[1].volume += Time.deltaTime / 2;
            if (music[0].volume > 0f)
                music[0].volume -= Time.deltaTime / 2;
        }
    }
}
