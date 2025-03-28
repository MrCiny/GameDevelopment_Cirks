using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiggleScript : MonoBehaviour
{
    [Range(0f, 1f)]
    public float power = 0.1f;

    [Header("Position")]
    public bool jigPos = true;
    public Vector3 posJigAmount;
    [Range(0, 100)]
    public float posFreq = 8;
    float posTime;

    [Header("Rotation")]
    public bool jigRot = true;
    public Vector3 rotJigAmount;
    [Range(0, 100)]
    public float rotFreq = 8;
    float rotTime;

    [Header("Scale")]
    public bool jigScale = true;
    public Vector3 scaleJigAmount = new Vector3(0.1f, -0.1f, 0.1f);
    [Range(0, 100)]
    public float scaleFreq = 8;
    float scaleTime;

    Vector3 basePosition;
    Quaternion baseRotation;
    Vector3 baseScale;

    void Start()
    {
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //var dt = Time.timeScale; will be paused if user timeScale = 0
        var dt = Time.unscaledDeltaTime;
        posTime += dt * posFreq;
        rotTime += dt * rotFreq;
        scaleTime += dt * scaleFreq;

        if (jigPos)
        {
            transform.localPosition = basePosition + posJigAmount * Mathf.Sin(posTime) * power;
        }

        if (jigRot)
        {
            transform.localRotation = baseRotation * Quaternion.Euler(rotJigAmount * Mathf.Sin(posTime) * power);
        }

        if (jigScale)
        {
            transform.localScale = baseScale + scaleJigAmount * Mathf.Sin(scaleTime) * power;
        }
    }
}
