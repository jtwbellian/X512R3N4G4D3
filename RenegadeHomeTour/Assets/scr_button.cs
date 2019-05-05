using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class scr_button : MonoBehaviour
{

    private float spring = 0.2f;
    private Vector3 startPos;
    private AudioSource audio;

    public string textOn = "On";
    public string textOff = "Off";
    public bool screwsIn = false;
    public bool screwsOut = false;
    public bool on = false;
    public UnityEvent turnOn;
    public UnityEvent turnOff;
    public OVRHapticsManager haptics;
    public GameObject lightMesh;
    public int lightMatIndex;

    public float offset;

    public AudioClip onSnd;
    public AudioClip offSnd;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        haptics = OVRHapticsManager.GetInstance();
        audio = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (transform.localPosition.x <= 0f && !on)
        {
            on = true;
            turnOff.Invoke();

            audio.PlayOneShot(onSnd);

            if (col.transform.CompareTag("RightHand"))
            {
                haptics.BuzzRight(VibrationForce.Light, 0.05f);
            }
            if (col.transform.CompareTag("LeftHand"))
            {
                haptics.BuzzLeft(VibrationForce.Light, 0.05f);
            }

            if (lightMesh != null)
            {
                lightMesh.
            }
        }
        else if (transform.localPosition.x >= offset && on)
        {
            on = false;
            turnOn.Invoke();

            audio.PlayOneShot(offSnd);

            if (col.transform.CompareTag("RightHand"))
            {
                haptics.BuzzRight(VibrationForce.Light, 0.05f);
            }
            if (col.transform.CompareTag("LeftHand"))
            {
                haptics.BuzzLeft(VibrationForce.Light, 0.05f);
            }
        }


    }


    void Update()
    {
        if (on && transform.localPosition.x < offset)
        {
            transform.Translate(new Vector3((spring) * Time.deltaTime, 0f, 0f), Space.Self);

            if (screwsIn)
            {
                transform.Rotate(new Vector3(-90f * Time.deltaTime, 0f, 0f));
            }
        }

        if (!on && transform.localPosition.x > 0f)
        {
            transform.Translate(new Vector3((-spring) * Time.deltaTime, 0f, 0f), Space.Self);

            if (screwsOut)
            {
                transform.Rotate(new Vector3(90f * Time.deltaTime, 0f, 0f));
            }
        }

    }
}
