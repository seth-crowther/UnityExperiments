using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public HandleGun gunScript;

    private TextMeshProUGUI ammoTracker;
    private int maxAmmo;

    void Start()
    {
        ammoTracker = gameObject.GetComponent<TextMeshProUGUI>();
        maxAmmo = gunScript.GetMaxAmmo();
    }

    void Update()
    {
        ammoTracker.text = gunScript.GetAmmo() + " / " + maxAmmo;
    }
}
