using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProj : MonoBehaviour
{
    private float timeAlive;
    private float lifespan;

    void Start()
    {
        timeAlive = 0f;
        lifespan = 3f;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > lifespan)
        {
            Destroy(gameObject);
        }
    }
}
