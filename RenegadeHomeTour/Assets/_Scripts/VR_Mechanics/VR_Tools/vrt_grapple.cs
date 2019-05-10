using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_grapple : VRTool
{
    [SerializeField]
    private bool canFire = true;
    private AudioSource audioSource;
    private Animator anim;
    private scr_grappleHook hook;

    // private Light light;
    private OVRHapticsManager hm;

    public Rigidbody bulletType;
    public float fireSpeed = 10f;
    public Transform gunBarrel;
    //public ParticleSystem muzzleFlash;

    public override void Init()
    {
        //light = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        hm = OVRHapticsManager.instance;
        anim.speed = 1f;
        //anim.Play("Grapple_Bones|SpoolForward", 0, 1f);
        //muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    public override void IndexRelease()
    {
        //if (light != null)
        //light.intensity = 0f;
        canFire = true;
    }

    public void Retract()
    {
        if (grabInfo.isGrabbed)
        {
            var mc = transform.root.GetComponent<VRMovementController>();
            mc.rigidBody.AddForce( (hook.transform.position - transform.position) * 100f, ForceMode.Impulse);
        }
    }

    public override void IndexTouch()
    {
        Rigidbody shot;

        //if (light != null)
        //{
         //   light.intensity = .5f;
            //Invoke("LightOff", 0.2f);
        //}

        shot = Instantiate(bulletType);

        hook = shot.GetComponent<scr_grappleHook>();
        hook.gun = this;

        if (audioSource != null)
            audioSource.Play();

       // if (anim != null)
        //    anim.Play("Fire", 0, 0.0f);

        anim.SetBool("CWSpin", true);

        if (hm != null)
            hm.Play(VibrationForce.Hard, GetGrabber().grabbedBy.m_controller, 0.15f);

        /*
        shot.transform.position = gunBarrel.position;
        shot.transform.rotation = gunBarrel.rotation;
        
        shot.velocity = transform.forward * fireSpeed;

        Destroy(shot.gameObject, 4f);*/
        canFire = false;
        /*
        if (tutorialGun)
        {
            GameManager.GetInstance().direc.Ping(PING.gunFired);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Clear();
            muzzleFlash.Play();
        }*/
        
    }

    /*public void Authenicate()
    {
        tutorialGun = false;
    }
    public void MakeTutorial()
    {
        tutorialGun = true;
    }

    public void LightOff()
    {
        light.intensity = 0f;
    }*/

    public override void ThumbRelease()
    {
        //throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnGrab()
    {
        base.OnGrab();
       // light.gameObject.SetActive(true);
    }

    public override void OnRelease()
    {
        base.OnRelease();
        //light.gameObject.SetActive(false);
    }
}
