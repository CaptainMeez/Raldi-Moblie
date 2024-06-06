using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerStats : MonoBehaviour
{
    public PlayerData data;
    [SerializeField] private bool loadOnAwake;
    private bool instanceLoadedAlready = false;
    public GameObject loadIndication;
    public bool showLoadIndication = false;

    void Start()
    {
        if (loadOnAwake)
            Load();
    }
    
    public void Save()
    {
        SaveLoadManager.SavePlayer(data);
    }

    public void Load()
    {
        data = SaveLoadManager.LoadPlayer(PlayerPrefs.GetInt("CurrentSave"));
    }

    public void ResetToDefaults()
    {
        data.beatHardMode = false;
        data.recordScore = 0;
        data.foundObjectasks = false;
        data.windowIsBroken = false;
        data.interactedWithNeil = false;
        data.highBooks = 0;
        data.modifhighBooks = 0;
        data.recordNormalNotebooks = 0;
        data.recordNormalExits = 0;
        data.recordTime = 9999;
        data.modifrecordTime = 9999;
        data.objectasks = new int[8]{0, 0, 0, 0, 0, 0, 0, 0};
        data.theEnd = false;
        data.storyModeWon = false;
        data.cowMode = false;
        data.ishaanTimeWahoo = false;
        data.ishaanModeUnlocked = false;
        data.ishaanMenu = false;
        data.canTalkIshaan = false;
        data.level = 1;
        data.headCosmetic = 0;
        data.bodyCosmetic = 0;
        data.backCosmetic = 0;
        data.dealtWithSolitary = false;
        Save();
    }

    public void TryLoad() // Use this function before trying to get / set values in a Start() function
    {
        if (!instanceLoadedAlready)
        {
            Load();
            instanceLoadedAlready = true;
        }
    }
}
