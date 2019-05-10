﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : EVActor
{
    private iSpecial_Grabbable grabbable;
    // Start is called before the first frame update
    void Start()
    {
        grabbable = GetComponent<iSpecial_Grabbable>();
        subscribesTo = AppliesTo.TOOLS;
    }

    public override void BeginEvent()
    {
        switch (myEvent.type)
        {
            case EV.ItemGrabbed:
                grabbable.Grab += EndEvent;
                break;
            case EV.ItemDropped:
                grabbable.Release += EndEvent;
                break;

            default:
                Debug.Log("ERROR: Unknown event " + myEvent.type);
                return;
        }
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

        CompleteEvent();
    }
}
