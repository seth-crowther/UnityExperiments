using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public Transform player;
    public Transform mainCam;
    public Vector3 playerOffset;

    void Start()
    {
        playerOffset = transform.position - player.position;    
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        //Vector3 rotation = new Vector3(-vertical, horizontal, 0) * sensitivity;

        transform.Rotate(new Vector3(-vertical, 0, 0) * sensitivity);
        transform.RotateAround(player.position, player.up, horizontal * sensitivity);

        //transform.position = player.position + playerOffset;
    }
}
