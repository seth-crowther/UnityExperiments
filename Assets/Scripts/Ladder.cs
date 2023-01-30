using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public PlayerStateManager stateMgr;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stateMgr.climbing = transform;
            stateMgr.ChangeState(stateMgr.climbingState);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStateManager stateMgr = other.GetComponent<PlayerStateManager>();
            stateMgr.climbing = null;
            stateMgr.ChangeState(stateMgr.fallingState);
        }
    }
}
