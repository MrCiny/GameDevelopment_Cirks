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
        if (!PlayerPrefs.HasKey("musicVolume") && !PlayerPrefs.HasKey("resWidth") && !PlayerPrefs.HasKey("resHeight"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetInt("resWidth", 640);
            PlayerPrefs.SetInt("resHeight", 480);
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
        Save();
    }

    public void ChangeFullscreen()
    {
        isFullscreen = fullscreenToggle.isOn;
        Screen.SetResolution(selectedResList[selectedRes].width, selectedResList[selectedRes].height, isFullscreen);
        Save();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    public void Save()
    {
        selectedRes = dropdown.value;
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.SetInt("resWidth", selectedRes > 0 ? selectedResList[selectedRes].width : 640);
        PlayerPrefs.SetInt("resHeight", selectedRes > 0 ? selectedResList[selectedRes].height : 480);
        PlayerPrefs.SetInt("isFullscreen", isFullscreen ? 1 : 0);
    }

    public void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        bool fullscreenMode = PlayerPrefs.GetInt("isFullscreen") == 1 ? true : false;
        fullscreenToggle.isOn = fullscreenMode;

        int width = PlayerPrefs.GetInt("resWidth");
        int height = PlayerPrefs.GetInt("resHeight");
        Screen.SetResolution(width, height, fullscreenMode);
    }
}
