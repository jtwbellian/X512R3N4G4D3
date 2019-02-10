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
    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
    }
}