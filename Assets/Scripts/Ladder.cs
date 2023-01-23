using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Changing to climbing state");
            PlayerStateManager stateMgr = other.GetComponent<PlayerStateManager>();
            stateMgr.climbing = transform;
            stateMgr.ChangeState(stateMgr.climbingState);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStateManager stateMgr = other.GetComponent<PlayerStateManager>();
            stateMgr.ChangeState(stateMgr.movingState);
        }
    }
}
