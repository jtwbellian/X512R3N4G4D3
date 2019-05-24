using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_gravField : MonoBehaviour
{
    public bool useGravity = false;
    public bool isAttractor = false;
    public Vector3 force = new Vector3(0, 0, 0);


    void OnTriggerStay(Collider col)
    {
        if (col.attachedRigidbody)
        {
            if (isAttractor)
            {
                force = (col.transform.position - transform.position).normalized * force.magnitude;
                col.attachedRigidbody.AddForce(force, ForceMode.VelocityChange);
            }
            else
                col.attachedRigidbody.AddForce(force, ForceMode.Acceleration);
        }
    }

    /*
    void OnTriggerEnter(Collider col)
    {
        if (useGravity)
        {
            var rb = col.transform.root.GetComponentInChildren<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (useGravity)
        {
            var rb = col.transform.root.GetComponentInChildren<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = false;
            }
        }
    }*/

     // Update is called once per frame
    void Update()
    {
        
    }
}
