using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_tablet : VRTool
{

    public Transform leftGrip, rightGrip, offsetTarget;
    public Rigidbody rb;


    public override void IndexRelease()
    {
    }

    public override void IndexTouch()
    {
    }

    public override void Init()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    public override void ThumbRelease()
    {
        throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        throw new System.NotImplementedException();
    }

    public override void OnGrab()
    {
        Debug.Log("Lux plate pickup");
        if (grabInfo.grabbedBy.CompareTag("RightHand"))
        {
            offsetTarget.SetParent(rightGrip);
            offsetTarget.localPosition = Vector3.zero;
        }
        else if (grabInfo.grabbedBy.CompareTag("LeftHand"))
        {
            offsetTarget.SetParent(leftGrip);
            offsetTarget.localPosition = Vector3.zero;
        }
        base.OnGrab();
    }

    public override void OnRelease()
    {
        offsetTarget.SetParent(transform);
        //transform.localPosition = Vector3.zero;
        base.OnRelease();
    }

}
