using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.TextCore.Text;
using System.IO;

public class SaveLoadScript : MonoBehaviour
{
    public string fileName = "saveFile.json";

    [Serializable]
    public class GameData
    {
        public int character;
        public string characterName;
    }

    public GameData data = new GameData();
    public void SaveGame(int character, string name)
    {
        data.character = character;
        data.characterName = name;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath+ "/"+fileName, json);
        Debug.Log("Game saved to: " + Application.persistentDataPath + "/" + fileName);
    }
    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded from: " + Application.persistentDataPath + "/" + fileName);
        } 
        else
        {
            Debug.LogWarning("Save file neeksiste");
        }
    }
}
