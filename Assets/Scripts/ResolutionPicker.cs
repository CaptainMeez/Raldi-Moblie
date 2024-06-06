using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionPicker : MonoBehaviour
{
    public OptionsManager rootMenu;
    private Resolution[] resolutions = new Resolution[] {new Resolution(1920, 1080), new Resolution(2560, 1440), new Resolution(3840, 2160), new Resolution(1280, 720), new Resolution(854, 480), new Resolution(640, 360), new Resolution(426, 240)};
    private int index;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // Give settingsmanager time to load

        int index = 0;

        foreach(Resolution resolution in resolutions)
        {
            if (SettingsManager.resolution.width == resolution.width && SettingsManager.resolution.height == resolution.height)
            {
                this.index = index;
            }

            index++;
        }
    }

    public void ChangeSelection(int select)
    {
        index += select;

        if (index > 6)
            index = 0;
        else if (index < 0)
            index = 6;

        PlayerPrefs.SetFloat("Screen_Width", resolutions[index].width);
        PlayerPrefs.SetFloat("Screen_Height", resolutions[index].height);

        rootMenu.resolutionText.text = PlayerPrefs.GetFloat("Screen_Width").ToString() + "x" + PlayerPrefs.GetFloat("Screen_Height").ToString();

        SettingsManager.UpdateSettings();
    }
}

[System.Serializable]
public class Resolution
{
    public int width;
    public int height;

    public Resolution(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}
