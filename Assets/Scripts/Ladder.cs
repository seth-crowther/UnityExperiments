using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public PlayerStateManager player;

    // When player enters the ladder trigger, change player state to climbing
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.ChangeState(player.climbingState);
        }
    }


    // When player leaves the ladder trigger, change player state to falling
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.ChangeState(player.fallingState);
        }
    }
}
