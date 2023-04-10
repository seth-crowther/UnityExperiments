using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI fpsCount;
    private float timeSinceLastUpdate;
    private float period = 0.25f;

    void Start()
    {
        fpsCount = gameObject.GetComponent<TextMeshProUGUI>();
        timeSinceLastUpdate = period;
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate > period)
        {
            fpsCount.text = "FPS: " + (1f / Time.deltaTime).ToString("0.0");
            timeSinceLastUpdate = 0f;
        }
    }
}
