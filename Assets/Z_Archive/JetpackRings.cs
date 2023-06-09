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

    private ParticleSystem right1;
    private ParticleSystem left1;
    private ParticleSystem right2;
    private ParticleSystem left2;

    private bool whichParticleEffect = true;

    void Start()
    {
        particleStartRotation = Quaternion.Euler(-90f, 0f, 0f);

        rightJetPos = transform.position + new Vector3(0.6f, -1.2f, 0f);
        leftJetPos = transform.position + new Vector3(-0.6f, -1.2f, 0f);

        GameObject r1 = Instantiate(jumpParticles, rightJetPos, particleStartRotation, transform);
        GameObject l1 = Instantiate(jumpParticles, leftJetPos, particleStartRotation, transform);
        GameObject r2 = Instantiate(jumpParticles, rightJetPos, particleStartRotation, transform);
        GameObject l2 = Instantiate(jumpParticles, leftJetPos, particleStartRotation, transform);

        right1 = r1.GetComponent<ParticleSystem>();
        left1 = l1.GetComponent<ParticleSystem>();
        right2 = r2.GetComponent<ParticleSystem>();
        left2 = l2.GetComponent<ParticleSystem>();

        right1.Stop();
        left1.Stop();
        right2.Stop();
        left2.Stop();
    }

    public void PlayParticles()
    {
        if (whichParticleEffect)
        {
            right1.Play();
            left1.Play();
        }
        else
        {
            right2.Play();
            left2.Play();
        }
    }

    public void StopParticles()
    {
        if (whichParticleEffect)
        {
            right1.Stop();
            left1.Stop();
        }
        else
        {
            right2.Stop();
            left2.Stop();
        }
        whichParticleEffect = !whichParticleEffect;
    }
}
