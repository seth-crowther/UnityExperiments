using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTracking : MonoBehaviour
{
    public Transform mainCam; // Main camera reference
    public Transform player; // Player reference
    public float angleToSpeaker; // Angle around the y axis from camera's view to the speaker

    private Vector3 cameraForward; // Vector perpendicular to y axis that points forward from camera
    private Vector3 dirToSpeaker; // Vector perpendicular to y axis that points from camera to speaker
    private AudioSource audioSource; // AudioSource component of the speaker

    private float range; // Max range of the audio produced by the speaker
    private float playerDist; // Distance of the player from the speaker
    private float maxVolume; // Max volume of audio produced by the speaker

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        range = audioSource.maxDistance;
        maxVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustAudioPan();
        AdjustAudioAttenuation();
    }

    // Adjusts which ear the speaker plays audio through depending on the direction of the camera
    private void AdjustAudioPan()
    {
        cameraForward = new Vector3(mainCam.transform.forward.x, 0f, mainCam.transform.forward.z).normalized;
        dirToSpeaker = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(mainCam.transform.position.x, 0f, mainCam.transform.position.z)).normalized;

        angleToSpeaker = Vector3.SignedAngle(cameraForward, dirToSpeaker, Vector3.up) * Mathf.Deg2Rad;
        audioSource.panStereo = Mathf.Sin(angleToSpeaker);
    }

    // Adjusts the volume of the speaker audio depending on the distance from the player.
    // Currently set to falloff linearly. Could change in the future.
    private void AdjustAudioAttenuation()
    {
        playerDist = Vector3.Distance(transform.position, player.position);
        audioSource.volume = -(1 / range) * maxVolume * playerDist + maxVolume;
    }
}
