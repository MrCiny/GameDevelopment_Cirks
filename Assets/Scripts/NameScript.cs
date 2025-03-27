using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameScript : MonoBehaviour
{
    TextMeshPro TMP;
    void Awake()
    {
        TMP = transform.Find("NameField").gameObject.GetComponent<TextMeshPro>();
    }



    // Update is called once per frame
    public void setPlayerName(string name)
    {
        TMP.text = name;
        TMP.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }
}
