using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackRings : MonoBehaviour
{
    private Vector3 rightJetPos;
    private Vector3 leftJetPos;
    private Quaternion particleStartRotation;

    public GameObject jumpParticles;
    public PlayerStateManager player;

    private ParticleSystem right;
    private ParticleSystem left;
    private ParticleSystem.EmissionModule rightEmission;
    private ParticleSystem.EmissionModule leftEmission;

    void Start()
    {
        particleStartRotation = Quaternion.Euler(-90f, 0f, 0f);

        rightJetPos = transform.position + new Vector3(0.6f, -1.2f, 0f);
        leftJetPos = transform.position + new Vector3(-0.6f, -1.2f, 0f);

        GameObject r = Instantiate(jumpParticles, rightJetPos, particleStartRotation, transform);
        GameObject l = Instantiate(jumpParticles, leftJetPos, particleStartRotation, transform);
        right = r.GetComponent<ParticleSystem>();
        left = l.GetComponent<ParticleSystem>();

        rightEmission = right.emission;
        leftEmission = left.emission;

        right.Play();
        left.Play();
    }

    void Update()
    {
        if (player.GetCurrentState() == player.hoverState)
        {
            
            rightEmission.enabled = true;
            leftEmission.enabled = true;
        }
        else
        {
            rightEmission.enabled = false;
            leftEmission.enabled = false;
        }
    }
}
