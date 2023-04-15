using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanShoot : MonoBehaviour
{
    private Vector3 target = Vector3.zero;
    private Vector3 startingPos;
    private float timeAlive;
    private float timeToTarget = 0.5f;

    void Start()
    {
        timeAlive = 0f;
        startingPos = transform.position;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (target != Vector3.zero)
        {
            transform.position = Vector3.Lerp(startingPos, target, Mathf.Clamp01(timeAlive / timeToTarget));
        }

        if (transform.position == target)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 value)
    {
        target = value;
    }
}
