using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerStateManager player;
    private Slider healthBar;

    void Start()
    {
        healthBar = GetComponent<Slider>();
        healthBar.value = player.health;
    }

    void Update()
    {
        healthBar.value = player.health;
    }
}
