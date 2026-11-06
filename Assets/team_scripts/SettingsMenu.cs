using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumeSlider;
    public TMP_Dropdown resolutionDropdown;

    private List<Resolution> resolutions = new List<Resolution>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildResolutionDropdown();

        volumeSlider.onValueChanged.AddListener(SetVolume);

        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        LoadSettings();
    }

    void LoadSettings(){
        float volume = PlayerPrefs.GetFloat("volume");

        SetVolume(volume);

        int resolutionIndex = PlayerPrefs.GetInt("resolution");

        SetResolution(resolutionIndex);

    }

    void BuildResolutionDropdown(){
        resolutions.Clear();
        List<string> options = new List<string>();

        foreach(Resolution res in resolutions){
            if(resolutions.Exists(r => r.width == res.width && r.height == res.height)) continue;

            resolutions.Add(res);
            options.Add(res.width + " x " + res.height);
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }

    public void SetVolume(float value){
        mixer.SetFloat("volume", value);
        volumeSlider.value = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void SetResolution(int index){
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        resolutionDropdown.value = index;

        PlayerPrefs.SetInt("resolution", index);
    }
}