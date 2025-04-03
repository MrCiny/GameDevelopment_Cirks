using System;
using System.IO;
using UnityEngine;

public class SaveLoadScript : MonoBehaviour
{
    public string fileName = "saveFile.json";
    public SettingsScript script;

    [Serializable]
    public class GameData
    {
        public float volume;
        public int resWidth;
        public int resHeight;
        public int isFullscreen;
    }

    public GameData data = new GameData();
    public void SaveGame()
    {
        script.Save();

        data.volume = PlayerPrefs.GetFloat("musicVolume");
        data.resWidth = PlayerPrefs.GetInt("resWidth");
        data.resHeight = PlayerPrefs.GetInt("resHeight");
        data.isFullscreen = PlayerPrefs.GetInt("isFullscreen");

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath+ "/"+fileName, json);
    }
    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<GameData>(json);
            PlayerPrefs.SetInt("isFullscreen", data.isFullscreen);
            PlayerPrefs.SetFloat("musicVolume", data.volume);
            PlayerPrefs.SetInt("resWidth", data.resWidth);
            PlayerPrefs.SetInt("resHeight", data.resHeight);
            script.Load();
        } 
        else
        {
            Debug.LogWarning("Save file neeksiste");
        }
    }
}
