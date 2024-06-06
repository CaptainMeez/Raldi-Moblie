using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCloser : MonoBehaviour
{
    public bool neilSave = false;

    public void Close()
    {
        if (neilSave)
            PlayerPrefs.SetFloat("NeilScarySave", 1);

        Application.Quit();

        #if UNITY_EDITOR
        UnityEngine.SceneManagement.SceneManager.LoadScene("Startup");
        #endif
    }
}
