using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HandleGun : MonoBehaviour
{
    [SerializeField] private LayerMask allowHit;
    public GameObject grenade;
    public PlayerStateManager player;
    public PASAudio speaker;

    private Camera mainCam;
    private Vector3 screenCentre;
    private RaycastHit hit;
    private Vector3 shootDir;
    private Vector3 aimPoint;
    private float aimDistance = 75f;
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

        // Handle reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.Reload();
        }

        if (Input.GetMouseButtonDown(0) && player.ammo > 0 && !player.isReloading) // If left click pressed
        {
            speaker.OnBeat();
            player.ammo--;
            player.EnterShootingState();
            GameObject toShoot = Instantiate(grenade, transform.position, Quaternion.identity);
            Rigidbody rb = toShoot.GetComponent<Rigidbody>();
            rb.AddForce(shootDir * shootForce, ForceMode.Impulse);
        }
    }
}
