﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_gun : VRTool
{
    private bool virgin = true;
    private bool canFire = true;
    private AudioSource audioSource;
    private Animator anim;
    private Light light;
    private OVRHapticsManager hm;

    public Rigidbody bulletType;
    public float fireSpeed = 10f;
    public Transform gunBarrel;


    public override void Init()
    {
        light = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        hm = OVRHapticsManager.instance;

    }

    public override void IndexRelease()
    {
        if (light != null)
            light.intensity = 0f;
        canFire = true;
    }

    public override void IndexTouch()
    {
        Rigidbody shot;

        if (light != null)
        {
            light.intensity = .5f;
            Invoke("LightOff", 0.2f);
        }

        shot = Instantiate(bulletType);

        if (audioSource != null)
            audioSource.Play();

        if (anim != null)
            anim.Play("Fire", 0, 0.0f);


        if (hm != null)
            hm.Play(VibrationForce.Hard, GetGrabber().grabbedBy.m_controller, 0.15f);


        shot.transform.position = gunBarrel.position;
        shot.transform.rotation = gunBarrel.rotation;

        shot.velocity = transform.forward * fireSpeed;

        Destroy(shot.gameObject, 4f);
        canFire = false;

        if (virgin)
        {
            GameManager.GetInstance().direc.Ping(PING.gunFired);
            virgin = false;
        }
    }

    public void LightOff()
    {
        light.intensity = 0f;
    }

    public override void ThumbRelease()
    {
        //throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        //throw new System.NotImplementedException();
    }

    public new void OnGrab()
    {
        GameManager.GetInstance().direc.Ping(PING.GunGrabbed);
        base.OnGrab();
        light.gameObject.SetActive(true);
    }

    public new void OnRelease()
    {
        GameManager.GetInstance().direc.Ping(PING.gunDropped);
        base.OnRelease();
        light.gameObject.SetActive(false);
    }
}
