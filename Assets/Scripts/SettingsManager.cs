using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static float RenderDistance = 500;
    public static float MouseSensitivity = 2;
    public static float FamilyFriendly = 0;
    public static float StreamerMode = 1;
    public static float DynamicFOV = 1;
    public static bool disableFarts;
    public static float FOV = 60;
    public static bool hudUsesPlayerColor = false;
    public static bool fullscreen = true;
    public static bool vsync = true;
    public static Resolution resolution;

    private string[] values = 
    {
        "RenderDistance",
        "MouseSensitivity",
        "FamilyFriendly",
        "DynamicFOV",
        "FOV",
        "Fullscreen",
        "Screen_Width",
        "Screen_Height",
        "StreamerMode",
        "HUDPlayerColor",
		"VSync"
    };

    private int[] defaultValues =
    {
        500,
        2,
        1,
        2,
        60,
        2,
        1920,
        1080,
        1,
        0,
		2
    };

    void Start()
    {
        for(int i = 0; i < values.Length; i++)
        {
            if (PlayerPrefs.GetFloat(values[i]) == 0)
                PlayerPrefs.SetFloat(values[i], defaultValues[i]);
        }

        UpdateSettings();
    }

    public static void UpdateSettings()
    {
        FOV = PlayerPrefs.GetFloat("FOV");
        RenderDistance = PlayerPrefs.GetFloat("RenderDistance");
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        FamilyFriendly = PlayerPrefs.GetFloat("FamilyFriendly");
        DynamicFOV = PlayerPrefs.GetFloat("DynamicFOV");
        fullscreen = PlayerPrefs.GetFloat("Fullscreen") == 2;
        vsync = PlayerPrefs.GetFloat("VSync") == 2;
        StreamerMode = PlayerPrefs.GetFloat("StreamerMode");
        resolution = new Resolution(Mathf.RoundToInt(PlayerPrefs.GetFloat("Screen_Width")), Mathf.RoundToInt(PlayerPrefs.GetFloat("Screen_Height")));
        hudUsesPlayerColor = PlayerPrefs.GetFloat("HUDPlayerColor") == 1;
        disableFarts = PlayerPrefs.GetFloat("DisableFarts") == 2;

		if (vsync)
		{
			Application.targetFrameRate = -1;
			QualitySettings.vSyncCount = 1;
		}
		else if (!vsync)
		{
			Application.targetFrameRate = 10000;
			QualitySettings.vSyncCount = 0;
		}
    }

    public void UpdateResolution()
    {
        if (fullscreen)
            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);
        else if (!fullscreen)
            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.Windowed);
    }
}