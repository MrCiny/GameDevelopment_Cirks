using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeScript : MonoBehaviour
{
    public FadeScript fadeScript;
    public SaveLoadScript saveLoadScript;
    public void CloseGame()
    {
        StartCoroutine(Delay("quit"));
    }

    public void GoToMenu()
    {
        StartCoroutine(Delay("menu"));
    }


    public IEnumerator Delay(string command)
    {
        if (string.Equals(command, "quit", System.StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeOut(0.1f);
            PlayerPrefs.DeleteAll();

            Application.Quit();
        }
        else if (string.Equals(command, "play", System.StringComparison.OrdinalIgnoreCase)){
            yield return fadeScript.FadeOut(0.1f);
            saveLoadScript.SaveGame();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "menu", System.StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeOut(0.1f);
            saveLoadScript.SaveGame();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
