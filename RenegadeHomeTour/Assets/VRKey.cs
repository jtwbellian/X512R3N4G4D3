using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKey : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public OVRHapticsManager hm;

    void Start()
    {
    }

    void OnTriggerEnter(Collider col)
    {
        OVRGrabber hand = col.GetComponent<OVRGrabber>();

        if (hand!= null)
        {
            hm.Play(VibrationForce.Medium, hand.m_controller, 0.20f);
        }

        if (clip != null)
            source.PlayOneShot(clip);
    }
}
