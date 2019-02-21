using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VRTool : MonoBehaviour
{
    private Rigidbody rb;
    private OVRGrabbable grabInfo;
    private bool indexDown = false;
    private bool thumbDown = false;

    public bool usesIndex = true;
    public bool usesThumb = false;

    public bool isHat = false;
    public float indexValue = 0f;

    [HideInInspector]
    public int hand; // 0 = primary 1 = secondary


    // Start is called before the first frame update
    void Start()
    {
        grabInfo = GetComponent<OVRGrabbable>();
        rb = GetComponent<Rigidbody>();
        Init();
    }

    void OnTriggerEnter(Collider col)
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!isHat && grabInfo.isGrabbed)
        {
            //transform.localPosition = grabInfo.grabbedTransform.position;

            switch (hand)
            {
                case 1: // right hand

                    indexValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);


                    // Index Finger Primary
                    if (indexValue > 0.5f && !indexDown)
                    {
                        IndexTouch();
                        indexDown = true;
                    }
                    else if(indexValue <= 0.5f && indexDown)
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

                    indexValue = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

                    // Index Finger Secondary
                    if (indexValue > 0.5f && !indexDown)
                    {
                        IndexTouch();
                        indexDown = true;
                    }
                    else if (indexValue <= 0.5f && indexDown)
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

    public abstract void Init();
    public abstract void IndexTouch();
    public abstract void IndexRelease();

    public abstract void ThumbTouch();
    public abstract void ThumbRelease();

    public bool isHeld()
    {
        return grabInfo.isGrabbed;
    }

}
