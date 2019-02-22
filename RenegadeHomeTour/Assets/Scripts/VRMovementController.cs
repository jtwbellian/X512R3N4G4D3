using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VRMovementController : MonoBehaviour
{
    private const float TOL = 0.05f;
    private const float MAX_VIGNETTE = 0.7f;
    private const float MIN_VIGNETTE = 0.2f;
    private const float MAX_SPEED = 20f;
    private float boostbar_width = 0.0f;
    public Rigidbody rigidBody;
    private Transform headPos;
    private float rechargeRate = 0.5f;
    private bool ReadyToSnapTurn = false;
    private Vignette vignette;

    private OVRGrabber[] grabbers;

    public float speed = 2f;
    public float boost = 1f;
    public float rotationRatchet = 45f;

    public Text velocityLabel;
    public Transform boostBar;
    public PostProcessVolume ppVolume;

    // Start is called before the first frame update
    void Start()
    {
        if (boostBar == null)
            Debug.Log("ERROR: Boost bar is null, dummy!");
        else
            boostbar_width = boostBar.localScale.x;

        grabbers = GetComponentsInChildren<OVRGrabber>();

        ppVolume.profile.TryGetSettings(out vignette);

        //body = GetComponentInChildren<IKPlayerController>().transform;
        //rb = body.GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        headPos = GetComponentInChildren<Camera>().transform;

    }

    // Update is called once per frame
    void Update()
    {

        // Use Boost Jets
        var stickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        if (Mathf.Abs(stickY) > 0f && boost > 0f)
        {

            rigidBody.AddForce(headPos.forward * (speed * stickY * boost), ForceMode.Force);
            boost -= Time.deltaTime * Mathf.Abs(stickY);
        }
        else if (boost < 1f)
        {
            boost += Time.deltaTime * rechargeRate;
        }

        boostBar.localScale = new Vector3(boostbar_width * boost, boostBar.localScale.y, boostBar.localScale.z);
        
        // Turn view 
        Vector3 euler = transform.rotation.eulerAngles;
        //Vector3 lastPos = body.transform.localPosition;
        //body.transform.localPosition = Vector3.zero;

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            if (ReadyToSnapTurn)
            {
                euler.y -= rotationRatchet;
                ReadyToSnapTurn = false;
            }
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            if (ReadyToSnapTurn)
            {
                euler.y += rotationRatchet;
                ReadyToSnapTurn = false;
            }
        }
        else
        {
            ReadyToSnapTurn = true;
        }

        //body.transform.localPosition = lastPos;
        transform.rotation = Quaternion.Euler(euler);

        var canVig = true;

        for(int i = 0; i < grabbers.Length - 1; i ++)
        {
            if (grabbers[i].grabbedObject is OVRClimbable)
            {
                canVig = false;
            }
        }

        if (vignette != null )
        {
            if (canVig)
                vignette.intensity.value = Mathf.Clamp(Mathf.Abs(rigidBody.velocity.magnitude) / 2f, MIN_VIGNETTE, MAX_VIGNETTE);
            else
                vignette.intensity.value = MAX_VIGNETTE / 2f;
        }
 
    }

}