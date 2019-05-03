using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_lock : MonoBehaviour
{
    private OVRGrabber lastGrabber = null;
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

    public void MoveTo(Vector3 pos, Vector3 rot)
    {
        currentx = transform.position.x;
        currenty = transform.position.y;
        currentz = transform.position.z;

        currentxr = transform.rotation.eulerAngles.x;
        currentyr = transform.rotation.eulerAngles.y;
        currentzr = transform.rotation.eulerAngles.z;

        newPos.Set(Mathf.Clamp(currentx, x[0], x[1]), Mathf.Clamp(currenty, y[0], y[1]), Mathf.Clamp(currentz, z[0], z[1]));
        //newRot.Set();

        rb.MovePosition(newPos);
        rb.MoveRotation(Quaternion.Euler(newRot));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (grabbable.isGrabbed)
        {
            transform.parent = null;

            lastGrabber = grabbable.grabbedBy;
            if (lastGrabber.m_parentHeldObject)
                lastGrabber.m_parentHeldObject = false;

            currentx = transform.position.x;
            currenty = transform.position.y;
            currentz = transform.position.z;

            currentxr = transform.rotation.eulerAngles.x;
            currentyr = transform.rotation.eulerAngles.y;
            currentzr = transform.rotation.eulerAngles.z;

            transform.position.Set(x ? oldPos.x : currentx, y ? oldPos.y : currenty, z ? oldPos.z : currentz);
            newRot.Set(xr ? oldRot.x : currentxr, yr ? oldRot.y : currentyr, zr ? oldRot.z : currentzr);
             
            //grabbable.grabbedBy.Lock(lockTarget);

            if (rb)
            {
                rb.MovePosition(newPos);
                rb.MoveRotation(Quaternion.Euler(newRot));
            }
            
            //transform.localPosition = newPos - transform.position;
            //transform.localRotation = Quaternion.Euler(newRot - transform.rotation.eulerAngles);
            
        }
        else if (lastGrabber != null)
        {
            lastGrabber.m_parentHeldObject = true;
            lastGrabber = null;
        }*/
    }
}
