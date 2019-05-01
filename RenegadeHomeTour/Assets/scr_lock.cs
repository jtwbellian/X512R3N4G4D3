using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_lock : MonoBehaviour
{
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

    public OVRGrabbable grabbable;
    public bool x, y, z, xr, yr, zr;

    // Start is called before the first frame update
    void Start()
    {
        if (!grabbable)
            grabbable = GetComponent<OVRGrabbable>();
        
        if (!rb)
            rb = GetComponent<Rigidbody>();

        oldPos = transform.position;
        oldRot = transform.rotation.eulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.isGrabbed)
        {
            currentx = transform.position.x;
            currenty = transform.position.y;
            currentz = transform.position.z;

            currentxr = transform.rotation.eulerAngles.x;
            currentyr = transform.rotation.eulerAngles.y;
            currentzr = transform.rotation.eulerAngles.z;

            newPos.Set(x ? oldPos.x : currentx, y ? oldPos.y : currenty, z ? oldPos.z : currentz);
            newRot.Set(xr ? oldRot.x : currentxr, yr ? oldRot.y : currentyr, zr ? oldRot.z : currentzr);

            if (rb)
            {
                rb.MovePosition(newPos);
                rb.MoveRotation(Quaternion.Euler(newRot));
            }
            else
            {
                transform.position = newPos;
                transform.rotation = Quaternion.Euler(newRot);
            }
        }
    }
}
