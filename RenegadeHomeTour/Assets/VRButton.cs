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

    void OnTriggerEnter(Collider otherCollider)
    {
        var isWeapon = otherCollider.transform.GetComponent<DoesDammage>();

        if (otherCollider.transform.CompareTag("RightHand") || otherCollider.transform.CompareTag("LeftHand") || isWeapon != null)
        {
            if (on)
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
            else
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

            canPush = false;
            Invoke("AllowPush", 15f);
        }

    }

    public void AllowPush()
    {
        canPush = true;
    }
}
