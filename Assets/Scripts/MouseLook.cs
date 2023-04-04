using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public Transform player;

    private void Start()
    {
        
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        Vector3 rotation = new Vector3(-vertical, horizontal, 0) * sensitivity;

        transform.Rotate(rotation);
    }
}
