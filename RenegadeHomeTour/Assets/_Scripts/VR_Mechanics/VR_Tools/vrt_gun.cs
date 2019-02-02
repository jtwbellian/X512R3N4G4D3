using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_gun : VRTool
{

    private bool canFire = true;
    public Rigidbody bulletType;
    public float fireSpeed = 10f;
    public Transform gunBarrel;

    public override void IndexRelease()
    {
        canFire = true;
    }

    public override void IndexTouch()
    {
        Rigidbody shot;

        shot = Instantiate(bulletType);

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

}
