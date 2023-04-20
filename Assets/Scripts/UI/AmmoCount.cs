using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public HandleGun gun;

    private TextMeshProUGUI ammoTracker;
    private int maxAmmo;

    void Start()
    {
        ammoTracker = gameObject.GetComponent<TextMeshProUGUI>();
        maxAmmo = gun.GetMaxAmmo();
    }

    void Update()
    {
        ammoTracker.text = gun.GetAmmo() + " / " + maxAmmo;
    }
}
