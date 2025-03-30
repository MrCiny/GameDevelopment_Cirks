using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider volumeSlider;

    Resolution[] allResolutions;
    bool isFullscreen;
    int selectedRes;
    float volume;
    List<Resolution> selectedResList = new List<Resolution>();
    // Start is called before the first frame update
    void Start()
    {
        isFullscreen = true;
        allResolutions = Screen.resolutions;

        List<string> resStringList = new List<string>();
        string newRes;
        foreach (Resolution res in allResolutions) 
        { 
            newRes = res.width.ToString() + " x " + res.height.ToString();
            if (!resStringList.Contains(newRes))
            {
                resStringList.Add(newRes);
                selectedResList.Add(res);
            }
        }

        dropdown.AddOptions(resStringList);
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeResolution()
    {
        selectedRes = dropdown.value;
        Screen.SetResolution(selectedResList[selectedRes].width, selectedResList[selectedRes].height, isFullscreen);
    }

    public void ChangeFullscreen()
    {
        isFullscreen = fullscreenToggle.isOn;
        Screen.SetResolution(selectedResList[selectedRes].width, selectedResList[selectedRes].height, isFullscreen);
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
}
