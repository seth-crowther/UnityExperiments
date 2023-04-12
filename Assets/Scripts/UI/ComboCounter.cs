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
        comboCount.text = combo.ToString();
    }
}
