using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        if (PlayerPrefs.GetFloat("GameFirstLoad") == 0)
        {
            PlayerPrefs.SetFloat("GameFirstLoad", 1);
        }

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(4.5f);

            SceneManager.LoadSceneAsync("Warning");
        }

        StartCoroutine(WaitTime());       
    }
}
