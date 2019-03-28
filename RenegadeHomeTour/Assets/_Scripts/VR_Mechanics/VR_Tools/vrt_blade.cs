using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_blade : VRTool
{
    private int orientation = 0;

    private Quaternion rotationDown;
    private Quaternion rotationUp;
    private Vector3 positionUp;
    private Vector3 positionDown;
    private DoesDammage dd;

    Transform knife;

    public override void Init()
    {
        knife = transform.GetChild(0);
        rotationDown = knife.localRotation;
        positionDown = knife.localPosition;
        rotationUp = knife.localRotation * Quaternion.Euler( 180f, 0f, 0f);
        positionUp = positionDown + new Vector3(0f, -0.1f, -0.0392f);
        orientation = 0;
        StopCoroutine("Flip");
        knife.rotation = rotationUp;
        StartCoroutine("Flip");
        dd = GetComponentInChildren<DoesDammage>();

    }

    public override void IndexRelease()
    {
        orientation = 0;
        StopCoroutine("Flip");
        knife.rotation = rotationUp;
        StartCoroutine("Flip");
    }

    public override void IndexTouch()
    {
        orientation = 1;
        StopCoroutine("Flip");
        knife.rotation = rotationDown;
        StartCoroutine("Flip");
    }

    public void OnGrab()
    {
        base.OnGrab();
        dd.Enable();
    }

    public void OnRelease()
    {

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

            knife.localRotation = Quaternion.Lerp(rotationDown, rotationUp, t);
            knife.localPosition = Vector3.Lerp(positionDown, positionUp, t);

            yield return new WaitForSeconds(0.01f);
        }

        StopCoroutine("Flip");
        yield return null;
    }
}
