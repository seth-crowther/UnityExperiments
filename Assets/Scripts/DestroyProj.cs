using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProj : MonoBehaviour
{
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
}
