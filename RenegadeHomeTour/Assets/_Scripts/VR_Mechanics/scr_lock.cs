using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_lock : iSpecial_Grabbable
{
    private Transform trueParent = null;
    private OVRGrabber lastGrabber = null;
    private float breakPoint = 1f;//.5f;
    private bool held = false;
    private Vector3 lastPos;
    private Vector3 lastRot;
    private Collider myCol;
    private Vector3 offset = Vector3.zero;
    private Vector3 handPos;

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
        Grab += OnGrab;
        Release += OnRelease;

        currentx = transform.position.x;
        currenty = transform.position.y;
        currentz = transform.position.z;
        currentxr = transform.localRotation.eulerAngles.x;
        currentyr = transform.localRotation.eulerAngles.y;
        currentzr = transform.localRotation.eulerAngles.z;

        if (!grabbable)
            grabbable = GetComponent<OVRGrabbable>();
        
        if (!rb)
            rb = lockTarget.GetComponentInChildren<Rigidbody>();

        oldPos = transform.position;
        oldRot = transform.rotation.eulerAngles;

        trueParent = transform.parent;

        myCol = GetComponent<Collider>();

        foreach (Collider col in grabbable.allColliders)
        {
            Physics.IgnoreCollision(col, myCol);
        }
    }

    void FixedUpdate()
    {
        if (grabbable != null && grabbable.grabbedBy != null)
        {
            lastPos = transform.position;
            lastRot = transform.localRotation.eulerAngles;
            handPos = grabbable.grabbedBy.transform.position;

            currentx = handPos.x + offset.x;
            currenty = handPos.y + offset.y;
            currentz = handPos.z + offset.z;

            if (Vector3.Distance(lastPos, grabbable.grabbedBy.transform.position) > breakPoint)
            {
                
                //grabbable.grabbedBy.GetPlayerRB().AddForce((lastPos - transform.position) / Time.deltaTime, ForceMode.VelocityChange);
                grabbable.grabbedBy.GrabEnd();
                return;
            }

            currentxr = -Mathf.Atan2(lastPos.z - handPos.z, handPos.y - lastPos.y) * (180 / Mathf.PI);
            //currentxr = Mathf.Clamp(currentxr, oldRot.x + xr[0], oldRot.x + xr[1]);

            currentyr = -Mathf.Atan2(lastPos.x - handPos.x, handPos.z - lastPos.z) * (180 / Mathf.PI);
            //currentyr = Mathf.Clamp(currentyr, oldRot.y + yr[0], oldRot.y + yr[1]);

            currentzr = -Mathf.Atan2(lastPos.x - handPos.x, handPos.y - lastPos.y) * (180 / Mathf.PI);
            //currentzr = Mathf.Clamp(currentzr, oldRot.z + zr[0], oldRot.z + zr[1]);

        }
        else
        {
            currentx = transform.position.x;
            currenty = transform.position.y;
            currentz = transform.position.z;
            currentxr = transform.localRotation.eulerAngles.x;
            currentyr = transform.localRotation.eulerAngles.y;
            currentzr = transform.localRotation.eulerAngles.z;
        }

        newPos.Set(Mathf.Clamp(currentx, oldPos.x + x[0], oldPos.x + x[1]), Mathf.Clamp(currenty, oldPos.y + y[0], oldPos.y + y[1]), Mathf.Clamp(currentz, oldPos.z + z[0], oldPos.z + z[1]));

        newRot.Set(Mathf.Clamp(currentxr, oldRot.x + xr[0], oldRot.x + xr[1]), Mathf.Clamp(currentyr, oldRot.y + yr[0], oldRot.y + yr[1]), Mathf.Clamp(currentzr, oldRot.z + zr[0], oldRot.z + zr[1]));

        //rb.velocity = Vector3.zero;
        rb.MovePosition(newPos);
        rb.MoveRotation(Quaternion.Euler(newRot));

        lastPos = newPos;
        lastRot = newRot;

        transform.SetParent(trueParent);
        /*if (grabbable.isGrabbed)
        {
            grabbable.grabbedBy.Lock(transform);
        }*/
    }


    public void OnGrab()
    {
        Debug.Log("Grabbed Lock");
        grabbable.grabbedBy.m_parentHeldObject = false;
        LinesOff();
      
        transform.SetParent( trueParent);
        Debug.Log("parent set to  " + transform.parent);
        held = true;
        offset = transform.position - grabbable.grabbedBy.transform.position;
    }

    public void OnRelease()
    {
        LinesOn();
        Debug.Log("Released Lock");
        grabbable.grabbedBy.m_parentHeldObject = true;
        held = false;
    }

    public void LinesOn()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        // Set interactable lines on or off
        foreach (Renderer r in renderers)
        {
            r.material.SetInt("_lineMode", 1);
        }
    }

    public void LinesOff()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        // Set interactable lines on or off
        foreach (Renderer r in renderers)
        {
            r.material.SetInt("_lineMode", 0);
        }
    }

}
