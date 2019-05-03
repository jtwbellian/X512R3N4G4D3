using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_lock : MonoBehaviour, iSpecial_Grabbable
{
    private OVRGrabber lastGrabber = null;
    private float breakPoint = .5f;
    private bool held = false;
    private Vector3 lastPos;
    private Vector3 lastRot;

    private Rigidbody rb;
    private Vector3 oldPos,
                    newPos,
                    oldRot,
                    newRot;
    private float currentx,
                  currenty,
                  currentz,
                  currentxr,
                  currentyr,
                  currentzr;

    public Transform lockTarget;
    public OVRGrabbable grabbable;
    public Vector2 x, y, z, xr, yr, zr;

    // Start is called before the first frame update
    void Start()
    {
        if (!grabbable)
            grabbable = GetComponent<OVRGrabbable>();
        
        if (!rb)
            rb = lockTarget.GetComponentInChildren<Rigidbody>();

        oldPos = transform.position;
        oldRot = transform.rotation.eulerAngles;
    }

    void FixedUpdate()
    {
        if (grabbable.grabbedBy != null)
        {
            lastPos = transform.position;
            lastRot = transform.rotation.eulerAngles;

            currentx = grabbable.grabbedBy.transform.position.x;
            currenty = grabbable.grabbedBy.transform.position.y;
            currentz = grabbable.grabbedBy.transform.position.z;

            if(Vector3.Distance(lastPos, grabbable.grabbedBy.transform.position) > breakPoint)
            {
                grabbable.grabbedBy.GrabEnd();
                return;
            }

            currentxr = grabbable.grabbedBy.transform.rotation.eulerAngles.x;
            currentyr = grabbable.grabbedBy.transform.rotation.eulerAngles.y;
            currentzr = grabbable.grabbedBy.transform.rotation.eulerAngles.z;

            newPos.Set(Mathf.Clamp(currentx, oldPos.x + x[0], oldPos.x + x[1]), Mathf.Clamp(currenty, oldPos.y + y[0], oldPos.y + y[1]), Mathf.Clamp(currentz, oldPos.z + z[0], oldPos.z + z[1]));
            newRot.Set(Mathf.Clamp(currentx, xr[0], xr[1]), Mathf.Clamp(currenty, yr[0], yr[1]), Mathf.Clamp(currentz, zr[0], zr[1]));

            //rb.velocity = Vector3.zero;
            rb.MovePosition(newPos);
            rb.MoveRotation(Quaternion.Euler(newRot));

            lastPos = newPos;
            lastRot = newRot;
        }
        /*if (grabbable.isGrabbed)
        {
            grabbable.grabbedBy.Lock(transform);
        }*/
    }


    public void OnGrab()
    {
        Debug.Log("Grabbed Lock");
        grabbable.grabbedBy.m_parentHeldObject = false;
        transform.parent = null;
        held = true;
    }

    public void OnRelease()
    {
        Debug.Log("Released Lock");
        grabbable.grabbedBy.m_parentHeldObject = true;
        held = false;
    }
}
