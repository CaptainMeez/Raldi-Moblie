using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCEESingleplayerScreen : MonoBehaviour
{
    private bool open = false;
    
    public AudioSource music;

    private void LateUpdate()
    {
        if (open && music.volume > 0)
            music.volume -= Time.deltaTime / 3;
    }

    public void Open()
    {
        open = true;

        IEnumerator WaitToDisable()
        {
            yield return new WaitForSeconds(9f);
            Screen.SetResolution(854, 480, FullScreenMode.FullScreenWindow);
            Application.Quit();
        }

        StartCoroutine(WaitToDisable());
    }
}
