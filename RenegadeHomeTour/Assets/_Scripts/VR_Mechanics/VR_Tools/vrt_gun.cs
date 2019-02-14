using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_gun : VRTool, iSpecial_Grabbable
{

    private bool canFire = true;
    private AudioSource audioSource;
    private Animator anim;

    public Rigidbody bulletType;
    public float fireSpeed = 10f;
    public Transform gunBarrel;
    

    public override void Init()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
    }

    public override void IndexRelease()
    {
        canFire = true;
    }

    public override void IndexTouch()
    {
        Rigidbody shot;

        shot = Instantiate(bulletType);

        if (audioSource != null)
            audioSource.Play();

        if (anim != null)
            anim.Play("Fire", 0, 0.0f);

        //shot.transform.parent = null;

        shot.transform.position = gunBarrel.position;
        shot.transform.rotation = gunBarrel.rotation;
        
        shot.velocity = transform.forward * fireSpeed;

        Destroy(shot.gameObject, 1f);
        canFire = false;
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
        throw new System.NotImplementedException();
    }

    void iSpecial_Grabbable.OnRelease()
    {
        throw new System.NotImplementedException();
    }
}
