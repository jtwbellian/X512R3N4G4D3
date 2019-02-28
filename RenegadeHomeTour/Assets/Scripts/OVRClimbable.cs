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

    void Start()
    {
        base.Start();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        Debug.Log("Climbable Grabbed");
        isHeld = true;
        LinesOff();

        base.GrabBegin(hand, grabPoint);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Debug.Log("Climbable Released");
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