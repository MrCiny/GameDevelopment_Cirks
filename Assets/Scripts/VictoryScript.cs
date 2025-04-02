using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    public GameObject resumeMenu;
    public GameObject victory;
    public GameObject victoryMenu;
    public GameObject leaderboard;
    public SceneChangeScript changeScene;

    public float delay;

    private void Start()
    {
        PlayerPrefs.SetInt("isVictory", 0);
    }

    public void Victory()
    {
        PlayerPrefs.SetInt("isVictory", 1);
        StartCoroutine(ToggleActiveCoroutine(delay, victoryMenu, resumeMenu));
    }

    public void ToggleLeaderboard()
    {
        StartCoroutine(ToggleActiveCoroutine(delay, victory, leaderboard));
    }

    public void MainMenu()
    {
        changeScene.GoToMenu();
    }

    private IEnumerator ToggleActiveCoroutine(float delay, GameObject gameObject1, GameObject gameObject2)
    {
        yield return new WaitForSeconds(delay);
        gameObject2.SetActive(!gameObject2.activeSelf);
        gameObject1.SetActive(!gameObject1.activeSelf);
    }
}
