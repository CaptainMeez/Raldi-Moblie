using System;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class OptionsManager : MonoBehaviour
{
    public Slider slider;
    public Slider rendDist;
    public Slider fov;

    public Toggle familyFriendly;
    public Toggle dynamFov;
    public Toggle fullscreen;
    public Toggle vsync;
    public Toggle playerHud;
    public Toggle streamerHud;
    public Toggle disableFarts;

    public TextMeshProUGUI resolutionText;

    public TextMeshProUGUI fovValue;
    public TextMeshProUGUI renderValue;

    private int menuSelected = 0;
    public GameObject[] menus;
    public Image[] buttons;

    private void Start()
    {
        foreach(GameObject menu in menus) {menu.SetActive(true);}

        rendDist.value = SettingsManager.RenderDistance;
        slider.value = SettingsManager.MouseSensitivity;
        resolutionText.text = PlayerPrefs.GetFloat("Screen_Width").ToString() + "x" + PlayerPrefs.GetFloat("Screen_Height").ToString();
        playerHud.isOn = SettingsManager.hudUsesPlayerColor;
        streamerHud.isOn = SettingsManager.StreamerMode == 2;
        disableFarts.isOn = SettingsManager.disableFarts;
        
        if (SettingsManager.FamilyFriendly == 2) familyFriendly.isOn = true;
        else if (SettingsManager.FamilyFriendly == 1) familyFriendly.isOn = false;

        if (SettingsManager.DynamicFOV == 2) dynamFov.isOn = true;
        else if (SettingsManager.FamilyFriendly == 1) dynamFov.isOn = false;

        fullscreen.isOn = SettingsManager.fullscreen;
        vsync.isOn = SettingsManager.vsync;

        SwitchMenu(0);
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", slider.value);
        PlayerPrefs.SetFloat("FOV", Mathf.RoundToInt(fov.value));
        PlayerPrefs.SetFloat("RenderDistance", Mathf.RoundToInt(rendDist.value));
        if (this.familyFriendly.isOn) PlayerPrefs.SetFloat("FamilyFriendly", 2);
        else PlayerPrefs.SetFloat("FamilyFriendly", 1);
        if (dynamFov.isOn) PlayerPrefs.SetFloat("DynamicFOV", 2);
        else PlayerPrefs.SetFloat("DynamicFOV", 1);
        if (fullscreen.isOn) PlayerPrefs.SetFloat("Fullscreen", 2);
        else PlayerPrefs.SetFloat("Fullscreen", 1);
		if (vsync.isOn) PlayerPrefs.SetFloat("VSync", 2);
        else PlayerPrefs.SetFloat("VSync", 1);
        if (disableFarts.isOn) PlayerPrefs.SetFloat("DisableFarts", 2);
        else PlayerPrefs.SetFloat("DisableFarts", 1);

        PlayerPrefs.SetFloat("HUDPlayerColor", playerHud.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("StreamerMode", streamerHud.isOn ? 2 : 1);
        
        fovValue.text = Mathf.RoundToInt(fov.value).ToString();
        renderValue.text = Mathf.RoundToInt(rendDist.value).ToString();

        SettingsManager.UpdateSettings();
    }

    public void SwitchMenu(int index)
    {
        menuSelected = index;

        for (int i = 0; i < menus.Length; i++) 
        {
            if (i == menuSelected)
            {
                menus[i].SetActive(true);
                buttons[i].color = Color.white;
            }
            else
            {
                menus[i].SetActive(false);
                buttons[i].color = Color.gray;
            }
        }
    }
}