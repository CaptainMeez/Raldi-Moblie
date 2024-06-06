using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PussyOutButton : MonoBehaviour
{
    public PlayerStats file;

    public void PussyOut()
    {
        PlayerPrefs.SetFloat("PussiedOut", 1);      
        file.data.interactedWithNeil = false;
        file.Save();

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
    }
}
