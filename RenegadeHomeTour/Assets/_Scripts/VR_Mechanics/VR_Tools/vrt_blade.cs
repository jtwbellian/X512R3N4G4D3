using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_blade : VRTool
{
    private int orientation = 0;
    [SerializeField]
    private Vector3 rotationUp;
    [SerializeField]
    private Vector3 rotationDown;
    [SerializeField]
    private Vector3 positionUp;
    [SerializeField]
    private Vector3 positionDown;
    private DoesDammage dd;
    public bool simulateDrag;

    Transform knife;

    public override void Init()
    {
        knife = transform.GetChild(0);
        //rotationDown = knife.localRotation.eulerAngles;
        //positionDown = knife.localPosition;
        //rotationUp = knife.localRotation.eulerAngles + new Vector3( 180f, 0f, 0f);
        //positionUp = positionDown + new Vector3(0f, -0.1f, -0.0392f);
        orientation = 0;
        //StopCoroutine("Flip");
        knife.rotation = Quaternion.Euler(rotationUp);
        //StartCoroutine("Flip");
        dd = GetComponentInChildren<DoesDammage>();
        if (simulateDrag && grabInfo.snapOrientation)
        {
            Debug.Log("Snap orientation must be off for simulate drag to work.");
        }
    }

    public override void IndexRelease()
    {
        orientation = 0;
        StopCoroutine("Flip");
        knife.rotation = Quaternion.Euler(rotationUp);
        StartCoroutine("Flip");
    }

    public override void IndexTouch()
    {
        orientation = 1;
        StopCoroutine("Flip");
        knife.rotation = Quaternion.Euler(rotationDown);
        StartCoroutine("Flip");
    }

    public override void OnGrab()
    {
        base.OnGrab();
        knife.localRotation = Quaternion.Euler(rotationUp);
        knife.localPosition = positionUp;
        dd.Enable();
    }

    void FixedUpdate()
    {
        if (simulateDrag && grabInfo.isGrabbed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, grabInfo.grabbedBy.transform.rotation, Mathf.Min(Time.deltaTime, grabInfo.grabbedBy.GetVelocity().magnitude));
        }
    }

    public override void OnRelease()
    {
        dd.Disable();

        if (orientation == 0)
        {
            var rb = GetRB();
            if (rb != null)
            {
                //Debug.Log("Knife Thrown");
                rb.AddForce(rb.velocity * 50f, ForceMode.Impulse);
                //rb.AddTorque(transform.up * 50f);
            }
            else Debug.Log("Could not find knife rigid body :(");
        }
        base.OnRelease();
    }


    public override void ThumbRelease()
    {
        //throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        //throw new System.NotImplementedException();
    }

    IEnumerator Flip()
    {
        for(float i = 0f; i < 1f; i += 0.1f)
        {
            // Kind of weird, but this works for going either backwards or forwards
            float t = Mathf.Abs(orientation - i);

            knife.localRotation = Quaternion.Lerp(Quaternion.Euler(rotationDown), Quaternion.Euler(rotationUp), t);
            knife.localPosition = Vector3.Lerp(positionDown, positionUp, t);

            yield return new WaitForSeconds(0.02f);
        }

        knife.localRotation = Quaternion.Lerp(Quaternion.Euler(rotationDown), Quaternion.Euler(rotationUp), 1 - orientation);
        knife.localPosition = Vector3.Lerp(positionDown, positionUp, 1 - orientation);
        StopCoroutine("Flip");
        yield return null;
    }
}
