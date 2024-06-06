using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeUpdater : MonoBehaviour
{
    [SerializeField] private float currentVolume;

    [SerializeField] public AudioMixer mixer;
    [SerializeField] public VolumeType type;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("DumbassAudioFailsave") == 0)
        {
            string[] setDefault = {"MusicVolume", "SoundVolume", "DumbassAudioFailsave"};
            
            // Set all values in the array to the default value (including the failsave since that prevents this code from being called again)
            foreach(string defaults in setDefault) { PlayerPrefs.SetFloat(defaults, 1); }
        }
    }
    
    public void Update()
    {
	    currentVolume = PlayerPrefs.GetFloat(GetSettingByType(type));
        mixer.SetFloat("Main", Mathf.Log10(currentVolume) * 20);
    }

    public static string GetSettingByType(VolumeType type)
    {
        return type.ToString() + "Volume";
    }
}