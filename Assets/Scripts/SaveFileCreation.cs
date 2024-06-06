using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveFileCreation : MonoBehaviour
{
    public PlayerStats stats;
    public InputField text;

    void Awake()
    {
        text.text = "";
    }


    void Update()
    {
        stats.data.name = text.text;
        stats.data.playercolor_r = 124;
        stats.data.playercolor_g = 217;
        stats.data.playercolor_b = 150;
    }

    public void SaveAndMenu()
    {
        stats.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
