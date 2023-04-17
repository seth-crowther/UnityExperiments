using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public PlayerStateManager player;

    private TextMeshProUGUI ammoTracker;
    private int maxAmmo;

    void Start()
    {
        ammoTracker = gameObject.GetComponent<TextMeshProUGUI>();
        maxAmmo = player.maxAmmo;
    }

    void Update()
    {
        ammoTracker.text = player.ammo + " / " + maxAmmo;
    }
}
