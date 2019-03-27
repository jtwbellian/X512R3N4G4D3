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

    void OnTriggerEnter()
    {
        if (clip != null)
            source.PlayOneShot(clip);
    }
}
