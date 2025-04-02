using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    private string filePath;
    private List<PlayerData> leaderboard = new List<PlayerData>();
    private const int maxEntries = 5;
    [SerializeField] public GameObject leaderboardItems;
    private void Awake()
    {
        filePath = Application.persistentDataPath + "/leaderboard.txt";
        LoadLeaderboard();
    }

    public void UpdateLeaderboard(string playerName)
    {
        LoadLeaderboard();
        PlayerData player = leaderboard.FirstOrDefault(p => p.name == playerName);

        if (player != null)
        {
            player.wins++;
        }
        else
        {
            leaderboard.Add(new PlayerData(playerName, 1));
        }

        SortAndSave();
    }

    private void SortAndSave()
    {
        filePath = Application.persistentDataPath + "/leaderboard.txt";
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (!File.Exists(filePath))
            File.Create(filePath).Close();

        leaderboard = leaderboard.OrderByDescending(p => p.wins).ToList();

        if (leaderboard.Count > maxEntries)
            leaderboard = leaderboard.Take(maxEntries).ToList();

        foreach (PlayerData player in leaderboard)
        {
            Debug.Log($"Player {player.name}");
        }

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            foreach (PlayerData p in leaderboard)
            {
                writer.WriteLine($"{p.name}:{p.wins}");
            }
        }



        LoadLeaderboard();
    }

    private void LoadLeaderboard()
    {
        filePath = Application.persistentDataPath + "/leaderboard.txt";
        if (!File.Exists(filePath))
            return;

        Transform[] placeholders = leaderboardItems.GetComponentsInChildren<Transform>(true);
        leaderboard.Clear();
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            Debug.Log(line);
        }
        int j = 1;
        for (int i = 0; j < placeholders.Length && i < lines.Count(); i++) 
        {
            Transform placeholder = placeholders[j];
            string[] parts = lines[i].Split(':');
            Debug.Log(placeholders.Length);
            if (parts.Length == 2 && int.TryParse(parts[1], out int wins))
            {
                leaderboard.Add(new PlayerData(parts[0], wins));
            }
            foreach (PlayerData p in leaderboard)
            {
                Debug.Log(p.name);
            }   
            TextMeshProUGUI playerText = placeholder.Find("Player").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI winText = placeholder.Find("Wins").GetComponent<TextMeshProUGUI>();
            Debug.Log(playerText.name);
            playerText.text = parts[0] != null ? parts[0] : "";
            winText.text = parts[1] != null ? parts[1] : "";
            j += 5;
        }

        leaderboard = leaderboard.OrderByDescending(p => p.wins).Take(maxEntries).ToList();
    }
}

[System.Serializable]
public class PlayerData
{
    public string name;
    public int wins;

    public PlayerData(string name, int wins)
    {
        this.name = name;
        this.wins = wins;
    }
}