using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_gun : VRTool, iSpecial_Grabbable
{


    private bool canFire = true;
    private AudioSource audioSource;
    private Animator anim;
    private Light light;

    public Rigidbody bulletType;
    public float fireSpeed = 10f;
    public Transform gunBarrel;


    public override void Init()
    {
        light = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();

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

        haptics.Play(VibrationForce.Hard, grabInfo.grabbedBy.m_controller, 0.15f);

        //shot.transform.parent = null;

        shot.transform.position = gunBarrel.position;
        shot.transform.rotation = gunBarrel.rotation;
        
        shot.velocity = transform.forward * fireSpeed;

        Destroy(shot.gameObject, 4f);
        canFire = false;
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

     void iSpecial_Grabbable.OnGrab()
    {
        LinesOff();
    }

    void iSpecial_Grabbable.OnRelease()
    {
        throw new System.NotImplementedException();
    }
}
