using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : EVActor
{
    private iSpecial_Grabbable grabbable;

    AudioSource audio = null;
    // Start is called before the first frame update
    void Start()
    {
        grabbable = GetComponent<iSpecial_Grabbable>();
        subscribesTo = AppliesTo.TOOLS;
        audio = GetComponent<AudioSource>();
    }

    public override void BeginEvent()
    {
        switch (myEvent.type)
        {
            case EV.ItemGrabbed:
                //Debug.Log("Grab Event Registered");
                var myGrab = grabbable.GetComponent<OVRGrabbable>();

                if (myGrab!= null && myGrab.isGrabbed)
                { 
                    if (audio != null)
                    {
                        audio.Play();
                    }

                    CompleteEvent();
                    break;
                }
                else
                    grabbable.Grab += EndEvent;
                    
                break;

            case EV.ItemDropped:

                grabbable.Release += EndEvent;
                break;

            default:
                Debug.Log("ERROR: Unknown event " + myEvent.type);
                return;
        }

        //Debug.Log("EventBegin " + grabbable.Grab.GetInvocationList());
    }

    public void EndEvent()
    {
        switch (myEvent.type)
        {
            case EV.ItemGrabbed:
                grabbable.Grab -= EndEvent;
                break;

            case EV.ItemDropped:
                grabbable.Release -= EndEvent;
                break;

            default:
                Debug.Log("ERROR: " + myEvent.type + " could not be applied to Prop");
                return;
        }
        
        if (audio != null)
        {
            audio.Play();
        }
        
        CompleteEvent();
    }
}
