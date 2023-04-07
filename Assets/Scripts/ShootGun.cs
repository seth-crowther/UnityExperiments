using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : MonoBehaviour
{
    public LayerMask allowHit;
    public GameObject grenade;
    public PlayerStateManager player;

    private Camera mainCam;
    private Vector3 screenCentre;
    private RaycastHit hit;
    private Vector3 shootDir;
    private Vector3 aimPoint;
    private float aimDistance = 1000f;
    private float shootForce = 50f;

    void Start()
    {
        mainCam = Camera.main;
        screenCentre = new Vector3(Screen.width / 2, Screen.height / 2);
    }

    void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(screenCentre);

        if (Physics.Raycast(ray, out hit, 1000f, allowHit))
        {
            shootDir = (hit.point - transform.position).normalized;
        }
        else
        {
            aimPoint = mainCam.transform.position + (mainCam.transform.forward * aimDistance);
            shootDir = (aimPoint - transform.position).normalized;
        }

        if (Input.GetMouseButtonDown(0)) // If left click pressed
        {
            player.EnterShootingState();
            GameObject toShoot = Instantiate(grenade, transform.position, Quaternion.identity);
            Rigidbody rb = toShoot.GetComponent<Rigidbody>();
            rb.AddForce(shootDir * shootForce, ForceMode.Impulse);
        }
    }
}
