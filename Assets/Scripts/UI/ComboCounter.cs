using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboCounter : MonoBehaviour
{
    private TextMeshProUGUI comboCount;
    public PASAudio audioScript;
    private int combo;

    public void AddCombo()
    {
        combo++;
    }

    public void SetCombo(int value)
    {
        combo = value;
    }

    void Start()
    {
        combo = 0;
        comboCount = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // If too much time has passed since the last hit, reset the combo counter
        if (combo > 0 && audioScript.GetTimeSinceLastHit() > audioScript.GetPeriod() + (2 * audioScript.GetOnBeatThreshold()))
        {
            combo = 0;
        }

        comboCount.text = combo.ToString();
    }
}
