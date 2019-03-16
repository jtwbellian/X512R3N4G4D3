﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VRMovementController : MonoBehaviour
{
    private const float TOL = 0.05f;
    private const float MAX_VIGNETTE = 0.85f;
    private const float MIN_VIGNETTE = 0.3f;
    private const float MAX_SPEED =8f;
    private float boostbar_width = 0.0f;
    public Rigidbody rigidBody;
    private Transform head;
    private float rechargeRate = 0.5f;
    private bool ReadyToSnapTurn = false;
    private Vignette vignette;
    private float boostRate = 0.5f;

    private OVRGrabber[] grabbers;

    public bool canBoost = false;
    public float speed = 2f;
    public float boost = 1f;
    //public float rotationRatchet = 45f;

    public Text velocityLabel;
    public Transform boostBar;
    public PostProcessVolume ppVolume;
    private GameManager gm;

    private Vector3 euler;
    private Vector3 lastPos; 

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        
        if (boostBar == null)
            Debug.Log("ERROR: Boost bar is null, dummy!");
        else
            boostbar_width = boostBar.localScale.x;
            
        grabbers = transform.root.GetComponentsInChildren<OVRGrabber>();
        
        ppVolume.profile.TryGetSettings(out vignette);

        //body = GetComponentInChildren<IKPlayerController>().transform;
        //rb = body.GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        head = Camera.main.transform; //GetComponentInChildren<Camera>().transform;

    }

    public void ViewRatchet(float amt)
    {
        euler = transform.rotation.eulerAngles;
        lastPos = head.position;

        euler.y += amt;
        transform.rotation = Quaternion.Euler(euler);
        transform.position += lastPos - head.position;
        ReadyToSnapTurn = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (canBoost)
        {
            // Use Boost Jets
            var stickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

            if (Mathf.Abs(stickY) > 0f && boost > 0f)//rigidBody.velocity.magnitude < MAX_SPEED)
            {

                if (gm.direc.trainingMode)
                {
                    GameManager.GetInstance().direc.Ping(PING.analogFwd);
                }

                rigidBody.AddForce(head.forward * (speed * stickY * (boost + 0.1f)), ForceMode.Force);
                boost -= Time.deltaTime * Mathf.Abs(stickY) * 0.25f;
            }
            else if (boost < 1f)
            {
                boost += Time.deltaTime * rechargeRate;
            }

            boostBar.localScale = new Vector3(boostbar_width * boost, boostBar.localScale.y, boostBar.localScale.z);

        }

        // Turn View Left
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            if (ReadyToSnapTurn)
            {
                ViewRatchet(-45f);
            }
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            if (ReadyToSnapTurn)
            {
                ViewRatchet(45f);
            }
        }
        else if (ReadyToSnapTurn == false)
        {
            ReadyToSnapTurn = true;
        }


        var canVig = true;

        if (grabbers[0].grabbedObject is OVRClimbable || grabbers[1].grabbedObject is OVRClimbable)
            canVig = false;

        if (vignette != null )
        {
            if (canVig)
                vignette.intensity.value = Mathf.Clamp(Mathf.Abs(rigidBody.velocity.magnitude) / 2f, MIN_VIGNETTE, MAX_VIGNETTE);
            else
                vignette.intensity.value = MAX_VIGNETTE / 2f;
        }
 
    }

    public void AllowBoost()
    {
        canBoost = true;
    }
        

}