using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretButton : MonoBehaviour
{
    public void Click()
    {
        PlayerPrefs.SetString("Troll", "troll");
        SceneManager.LoadSceneAsync("School");
    }
}
