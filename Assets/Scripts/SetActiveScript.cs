using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveScript : MonoBehaviour
{
    public GameObject leaderboard;
    public GameObject settings;
    public GameObject buttons;
    public GameObject characterSelection;

    public void ToggleSettings(float delay)
    {
        StartCoroutine(setSettingsState(delay));
    }

    public void ToggleLeaderboard(float delay)
    {
        StartCoroutine(setLeaderboardState(delay));
    }
    public void ToggleCharSel(float delay)
    {
        StartCoroutine(setCharacterSelectionState(delay));
    }

    private IEnumerator setSettingsState(float delay)
    {
        yield return new WaitForSeconds(delay);
        buttons.SetActive(!buttons.activeSelf);
        settings.SetActive(!settings.activeSelf);
    }

    private IEnumerator setLeaderboardState(float delay)
    {
        yield return new WaitForSeconds(delay);
        buttons.SetActive(!buttons.activeSelf);
        leaderboard.SetActive(!leaderboard.activeSelf);
    }

    private IEnumerator setCharacterSelectionState(float delay)
    {
        yield return new WaitForSeconds(delay);
        buttons.SetActive(!buttons.activeSelf);
        characterSelection.SetActive(!characterSelection.activeSelf);
    }
}
