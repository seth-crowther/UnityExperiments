using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public Transform player;

    private Camera mainCam;
    private float mainCamFOV;
    private float maxAngle;

    private void Start()
    {
        mainCam = Camera.main;
        mainCamFOV = mainCam.fieldOfView;
        maxAngle = mainCamFOV + 10f; 
        transform.eulerAngles = Vector3.zero;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        float verticalAngleToRotate = -vertical * sensitivity;

        float nextAngle = transform.eulerAngles.x + verticalAngleToRotate;
        if (nextAngle > 180)
        {
            nextAngle -= 360;
        }

        // Clamping player's vertical look rotation
        if (nextAngle < -maxAngle)
        {
            transform.eulerAngles = new Vector3(-maxAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if (nextAngle > maxAngle)
        {
            transform.eulerAngles = new Vector3(maxAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            transform.RotateAround(transform.position, mainCam.transform.right, verticalAngleToRotate);
        }

        transform.RotateAround(player.position, player.up, horizontal * sensitivity);
    }
}
