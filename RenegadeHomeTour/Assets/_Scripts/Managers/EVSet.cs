using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVSet : EVActor
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        subscribesTo = AppliesTo.ENV;
    }

    public override void BeginEvent()
    {
        switch (myEvent.type)
        {
            case EV.ItemGrabbed:
                target.SetActive(true);
                CompleteEvent();
                break;

            case EV.ItemDropped:
                target.SetActive(false);
                CompleteEvent();
                break;

            default:
                Debug.Log("ERROR: Unknown event " + myEvent.type);
                return;
        }

        //Debug.Log("EventBegin " + grabbable.Grab.GetInvocationList());
    }
}
