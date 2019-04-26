using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{
    public bool on = false;
    private Material mat;
    public UnityEvent turnOn;
    public UnityEvent turnOff;
    public Light lightSignal;
    public bool canPush = true;
    public AudioClip onSound;
    public AudioClip offSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        lightSignal = GetComponentInChildren<Light>();

        mat = GetComponent<Renderer>().material;
        audioSource = GetComponent<AudioSource>();
    }

    void OnColliderEnter(Collider other)
    {
        OnTriggerEnter(other);
    }

    void OnTriggerEnter(Collider other)
    {
        var isWeapon = other.transform.GetComponent<DoesDammage>();

        if (other.transform.CompareTag("RightHand") || other.transform.CompareTag("LeftHand") || isWeapon != null)
        {
            if (on)
            {
                Off();
            }
            else
            {
                On();
            }

            canPush = false;
            Invoke("AllowPush", 15f);
        }
    }
    
    public void On()
    {
        on = true;

        mat.SetColor("_color", Color.green);
        turnOn.Invoke();

        if (lightSignal != null)
            lightSignal.color = Color.green;

        if (audioSource != null)
        {
            audioSource.clip = onSound;
            audioSource.Play();
        }

    }


    public void Off()
    {
        on = false;

        mat.SetColor("_color", Color.red);
        turnOff.Invoke();


        if (lightSignal != null)
            lightSignal.color = Color.red;

        if (audioSource != null)
        {
            audioSource.clip = offSound;
            audioSource.Play();
        }
    }

    public void AllowPush()
    {
        canPush = true;
    }
}
