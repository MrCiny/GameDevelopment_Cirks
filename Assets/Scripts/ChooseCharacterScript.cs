using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterScript : MonoBehaviour
{
    public GameObject[] characters;
    int characterIndex;
    public GameObject inputField;
    string charName;
    public int playerCount = 2;
    public SceneChangeScript sceneChangeScript;

    void Awake()
    {
        characterIndex = 0;
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }

        characters[characterIndex].SetActive(true);
    }

    public void NextCharacter()
    {
        characters[characterIndex].SetActive(false);
        characterIndex++;
        if (characterIndex == characters.Length)
            characterIndex = 0;

        characters[characterIndex].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[characterIndex].SetActive(false);
        characterIndex--;
        if (characterIndex == 0)
            characterIndex = characters.Length - 1;

        characters[characterIndex].SetActive(true);
    }

    public void Play()
    {
        charName = inputField.GetComponent<TMPro.TMP_InputField>().text;
        if(charName.Length > 2)
        {
            PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
            PlayerPrefs.SetString("PlayerName", charName);
            PlayerPrefs.SetInt("PlayerCount", playerCount);
            StartCoroutine(sceneChangeScript.Delay("play", characterIndex, name));
        } 
        else
        {
            inputField.GetComponent<TMPro.TMP_InputField>().Select();
        }
    }
}
