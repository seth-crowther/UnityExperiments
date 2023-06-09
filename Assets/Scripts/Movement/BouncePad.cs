using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    PlayerStateManager player;

    void Start()
    {
        player = FindObjectOfType<PlayerStateManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            PlayerBouncePadState.SetBouncePad(gameObject);
            player.ChangeState(PlayerStateManager.PlayerState.bouncePadState);
        }
    }
}
