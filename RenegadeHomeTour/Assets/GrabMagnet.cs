using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrabMagnet : MonoBehaviour
{
    public bool holdsHat;
    public bool holdsTool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        VRTool item;
    
        item = col.GetComponent<VRTool>();

        if (item == null)
            return;

        if (item.isHat && !holdsHat)
            return;
       /*
        if (holdsHat && !item.isHat)
            return;
 
        if (!holdsTool && !item.isHat)
            return;
            */

        // grab a tool 
        if (!item.isHeld() && col.transform.parent != transform)
        {
            Collider [] allCol = col.transform.GetComponentsInChildren<Collider>();
            Rigidbody rb = col.transform.GetComponent<Rigidbody>();

            foreach (Collider c in allCol)
            {
                c.isTrigger = true;
            }

            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            rb.isKinematic = true;

            item.transform.parent = transform;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }


}
