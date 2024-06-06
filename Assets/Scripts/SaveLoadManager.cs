using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
    public static void SavePlayer(PlayerData data)
    {
        string folderPath = GetCurrentPlatformPath();

        Directory.CreateDirectory(folderPath + "/RaldiSaves");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream1 = new FileStream(folderPath + "/RaldiSaves/" + PlayerPrefs.GetInt("CurrentSave").ToString() + ".raldi", FileMode.Create);

        bf.Serialize(stream1, data);
        stream1.Close();
        Debug.Log("Saved!");

        if (GameObject.FindObjectOfType<PlayerStats>().showLoadIndication)
            GameObject.Instantiate(GameObject.FindObjectOfType<PlayerStats>().loadIndication);
    }

    public static PlayerData LoadPlayer(int saveid)
    {
        string folderPath = GetCurrentPlatformPath();

        if(!File.Exists(folderPath + "/RaldiSaves/" + saveid + ".raldi")) 
            File.Create(folderPath + "/RaldiSaves/" + saveid + ".raldi"); 
        

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream1 = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/RaldiSaves/" + saveid + ".raldi", FileMode.Open);
        PlayerData data = bf.Deserialize(stream1) as PlayerData;

        if (data == null)
            Debug.LogError("An error occured while loading save file '" + saveid + "' (File might not be compatible with current version?)");

        stream1.Close();
        return data;
    }

    public static string GetCurrentPlatformPath()
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        #if UNITY_STANDALONE_OSX 
        folderPath = Application.persistentDataPath; 
        #endif
        
        return folderPath;
    }
}

[Serializable]
public class PlayerData
{
    public string name = "-";
    public bool beatHardMode = false;
    public int recordScore = 0;
    public bool foundObjectasks = false;
    public bool windowIsBroken = false;
    public bool interactedWithNeil = false;
    public bool dealtWithSolitary = false;
    public int highBooks = 0;
    public int modifhighBooks = 0;
    public int recordNormalNotebooks = 0;
    public int recordNormalExits = 0;
    public float recordTime = 9999;
    public float modifrecordTime = 9999;
    public int[] objectasks = {0, 0, 0, 0, 0, 0, 0, 0};
    public bool theEnd = false;
    public bool storyModeWon = false;
    public bool cowMode = false;
    public bool ishaanTimeWahoo = false;
    public bool ishaanModeUnlocked = false;
    public bool ishaanMenu = false;
    public bool canTalkIshaan = false;
    public int playercolor_r = 124;
    public int playercolor_g = 217;
    public int playercolor_b = 150;
    public int level = 1;
    public int headCosmetic = 0;
    public int bodyCosmetic = 0;
    public int backCosmetic = 0;
}