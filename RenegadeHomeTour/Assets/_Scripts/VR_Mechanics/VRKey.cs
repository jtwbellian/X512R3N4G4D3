using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKey : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public OVRHapticsManager hm;

    private OVRGrabber lastHand;

    void Start()
    {
    }

    void OnTriggerEnter(Collider col)
    {
        var rightTag = col.CompareTag("RightHand");
        var leftTag = col.CompareTag("LeftHand");
        var hand = col.GetComponent<OVRGrabber>();

        if (hand != null)
        {
            hm.Play(VibrationForce.Medium, hand.m_controller, 0.20f);
        }
        else if (rightTag) 
        {
            hm.Play(VibrationForce.Medium, hm.handR.m_controller, 0.20f);
        }
        else if (leftTag)
        {
            hm.Play(VibrationForce.Medium, hm.handL.m_controller, 0.20f);
        }

        if (clip != null)
            source.PlayOneShot(clip);
    }
}
