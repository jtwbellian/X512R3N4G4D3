using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VRTool : MonoBehaviour
{
    private OVRGrabbable grabInfo;
    private bool indexDown = false;
    private bool thumbDown = false;

    [HideInInspector]
    public int hand; // 0 = primary 1 = secondary


    // Start is called before the first frame update
    void Start()
    {
        grabInfo = GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {

        if (grabInfo.isGrabbed)
        {
            //transform.localPosition = grabInfo.grabbedTransform.position;

            switch (hand)
            {
                case 1: // right hand
                    // Index Finger Primary
                    if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f && !indexDown)
                    {
                        IndexTouch();
                        indexDown = true;
                    }
                    else if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) <= 0.5f && indexDown)
                    {
                        IndexRelease();
                        indexDown = false;
                    }

                    // Thumb Primary
                    if (OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) && ! thumbDown)
                    {
                        ThumbTouch();
                        thumbDown = true;
                    }
                    else if (!OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) && thumbDown)
                    {
                        ThumbRelease();
                        thumbDown = false;
                    }
                    break;

                case 0: // left hand
                    // Index Finger Secondary
                    if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f && !indexDown)
                    {
                        IndexTouch();
                        indexDown = true;
                    }
                    else if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) <= 0.5f && indexDown)
                    {
                        IndexRelease();
                        indexDown = false;
                    }

                    // Thumb Primary
                    if (OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons) && !thumbDown)
                    {
                        ThumbTouch();
                        thumbDown = true;
                    }
                    else if (!OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons) && thumbDown)
                    {
                        ThumbRelease();
                        thumbDown = false;
                    }

                    break;
            }
         }
    }

    public abstract void IndexTouch();
    public abstract void IndexRelease();

    public abstract void ThumbTouch();
    public abstract void ThumbRelease();

}
