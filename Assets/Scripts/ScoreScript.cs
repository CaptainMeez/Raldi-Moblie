using System;
using TMPro;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class ScoreScript : MonoBehaviour
{
    private void Start()
    {
        /*if (PlayerPrefs.GetString("CurrentMode") == "endless")
        {
            //this.scoreText.SetActive(true);
            this.text.text = "Score:\n" + PlayerPrefs.GetInt("CurrentBooks") + " Notebooks";
        }*/
    }
    public GameObject scoreText;
    public TMP_Text text;
}
