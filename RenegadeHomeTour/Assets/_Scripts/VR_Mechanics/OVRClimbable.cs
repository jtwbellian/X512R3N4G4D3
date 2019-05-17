/*
 * OVRClimbable - written by James Bellian
 * 
 * Used with and edited OVRGrabber to allow for "climbable" rather than
 * grabbable objects.
 * 
 * Binari Studios 2019
 */


using System;
using UnityEngine;


public class OVRClimbable : OVRGrabbable
{
    public bool isHeld = false;
    public Renderer[] renderers;
    private Collider playerBody;

    void Awake()
    {
        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if (collider == null)
            {
                throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }

        Physics.IgnoreLayerCollision(12, 12);
    }

    new void Start()
    {
        base.Start();

        renderers = GetComponentsInChildren<Renderer>();
        playerBody = Camera.main.transform.root.GetComponentInChildren<IKPlayerController>().transform.GetComponent<Collider>();
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        //Debug.Log("Climbable Grabbed");
        isHeld = true;
        base.GrabBegin(hand, grabPoint);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        //Debug.Log("Climbable Released");
        isHeld = false;
        base.GrabEnd(linearVelocity, angularVelocity);
    }
    

    void OnTriggerStay(Collider col)
    {
        if (isHeld)
            return;

        if ((col.CompareTag("LeftHand") || col.CompareTag("RightHand")))
        {
            LinesOn();
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if (isHeld)
            return;

        if ((col.CompareTag("LeftHand") || col.CompareTag("RightHand")))
        {
            LinesOff();
        }
    }

    public void LinesOn()
    {
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