using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hat : VRTool, iSpecial_Grabbable
{
    private SphereCollider myCollider;
    public SphereCollider hatSpot;

    void iSpecial_Grabbable.OnGrab()
    {
    }

    void iSpecial_Grabbable.OnRelease()
    {
        /*Debug.Log("Hat Drop");

        if (myCollider.bounds.Intersects(hatSpot.bounds))
        {
            Debug.Log("Released on Head");
            transform.parent = hatSpot.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<SphereCollider>();
        isHat = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void IndexTouch()
    {
        throw new System.NotImplementedException();
    }

    public override void IndexRelease()
    {
        throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        throw new System.NotImplementedException();
    }

    public override void ThumbRelease()
    {
        throw new System.NotImplementedException();
    }
}
