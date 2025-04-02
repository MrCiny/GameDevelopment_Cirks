using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseCanva;
    public GameObject resumeCanva;
    public GameObject pauseMenu;
    public GameObject settings;
    public GameObject leaderboard;
    public SceneChangeScript changeScene;
    public float delay;
    private bool isPaused = false;

    private void Start()
    {
        PlayerPrefs.SetInt("isPaused", 0);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && PlayerPrefs.GetInt("isVictory") == 0)
        {
            isPaused = !isPaused;
            int boolInt = isPaused ? 1 : 0;
            PlayerPrefs.SetInt("isPaused", boolInt);
            StartCoroutine(ToggleActiveCoroutine(delay, pauseCanva, resumeCanva));
        }
    }

    public void MainMenu()
    {
        changeScene.GoToMenu();
    }

    public void ResumeGame()
    {
        isPaused = !isPaused;
        int boolInt = isPaused ? 1 : 0;
        PlayerPrefs.SetInt("isPaused", boolInt);
        StartCoroutine(ToggleActiveCoroutine(delay, pauseCanva, resumeCanva));
    }

    public void SetActiveSettings()
    {
        StartCoroutine(ToggleActiveCoroutine(delay, pauseMenu, settings));
    }

    public void SetActiveLeaderboard()
    {
        StartCoroutine(ToggleActiveCoroutine(delay, pauseMenu, leaderboard));
    }

    private IEnumerator ToggleActiveCoroutine(float delay, GameObject gameObject1, GameObject gameObject2)
    {
        yield return new WaitForSeconds(delay);
        gameObject2.SetActive(!gameObject2.activeSelf);
        gameObject1.SetActive(!gameObject1.activeSelf);
    }
}
