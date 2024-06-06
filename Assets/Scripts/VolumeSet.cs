using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum VolumeType {Music, Sound}

public class VolumeSet : MonoBehaviour
{
    private float defaultValue = 1f;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private VolumeType type;

    // Function to be passed through the slider value changed event.
    public void SetLevel(float sliderValue) => SetVolume(sliderValue);

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(VolumeUpdater.GetSettingByType(type), 0.75f);
    }
    
    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat(VolumeUpdater.GetSettingByType(type), volume);
        mixer.SetFloat("Main", Mathf.Log10(volume) * 20);
    }

    public void Reset()
    {
        SetVolume(defaultValue);
        slider.value = defaultValue;
    }
}

