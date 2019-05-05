using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class scr_button : MonoBehaviour
{

    private float spring = 0.2f;
    private Vector3 startPos;
    private AudioSource audio;
    private Color onColor = Color.green;
    private Color offColor = Color.red;

    public string textOn = "On";
    public string textOff = "Off";
    public bool screwsIn = false;
    public bool screwsOut = false;
    public bool on = false;
    public UnityEvent turnOn;
    public UnityEvent turnOff;
    public OVRHapticsManager haptics;
    public Text textField;
    public MeshRenderer lightMesh;
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

        onColor = new Color(0f, 142f, 7f);
        offColor = new Color(255f, 13f, 0f);
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (transform.localPosition.x <= 0f && !on)
        {
            if (col.transform.CompareTag("RightHand"))
            {
                haptics.BuzzRight(VibrationForce.Light, 0.05f);
            }
            else if (col.transform.CompareTag("LeftHand"))
            {
                haptics.BuzzLeft(VibrationForce.Light, 0.05f);
            }

            Turn(true);
        }
        else if (transform.localPosition.x >= offset && on)
        {
            if (col.transform.CompareTag("RightHand"))
            {
                haptics.BuzzRight(VibrationForce.Light, 0.05f);
            }
            else if (col.transform.CompareTag("LeftHand"))
            {
                haptics.BuzzLeft(VibrationForce.Light, 0.05f);
            }

            Turn(false);
        }
    }

    private void Turn(bool isOn)
    {
        on = isOn;

        if (isOn)
            turnOff.Invoke();
        else
            turnOn.Invoke();

        audio.PlayOneShot(isOn?onSnd:offSnd);

        if (lightMesh != null)
        {
            lightMesh.materials[lightMatIndex].SetColor("_EmissionColor", isOn?onColor:offColor);
        }
        if (textField != null)
        {
            textField.color = isOn?onColor:offColor;
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
