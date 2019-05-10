using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_tablet : VRTool
{
    private float distFromHead = 0.5f;
    public Transform leftGrip, rightGrip, offsetTarget;
    public LiveCam selfiCam;

    public override void IndexRelease()
    {
    }

    public override void IndexTouch()
    {
    }

    public override void Init()
    {
        Invoke("GetInFace", 1f);
    }

    public void GetInFace()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distFromHead;
        transform.LookAt(Camera.main.transform, Vector3.up);
        transform.Rotate(new Vector3(1.0f, 0, 0), 90);
        offsetTarget.localRotation = Quaternion.Euler(-45,0,0);
    }

    public override void ThumbRelease()
    {
    }

    public override void ThumbTouch()
    {
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

        LiveCam.SetActive(selfiCam);

    }

    public override void OnRelease()
    {
        offsetTarget.SetParent(transform);
        //transform.localPosition = Vector3.zero;
        base.OnRelease();
    }

}
