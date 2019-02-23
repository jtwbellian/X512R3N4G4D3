using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(OVRGrabbable))]
public abstract class VRTool : MonoBehaviour
{
    private Rigidbody rb;
    private Collider[] toolCols;
    private OVRGrabbable grabInfo;
    //private Renderer[] renderers;

    private bool indexDown = false;
    private bool thumbDown = false;

    public bool usesIndex = true;
    public bool usesThumb = false;

    public bool isHat = false;
    public float indexValue = 0f;
    public GrabMagnet home;

    [HideInInspector]
    public int hand; // 0 = primary 1 = secondary

    // Start is called before the first frame update
    void Start()
    {
        toolCols = GetComponentsInChildren<Collider>();

        grabInfo = GetComponent<OVRGrabbable>();
        rb = GetComponent<Rigidbody>();
        Init();
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
                    else if (indexValue <= 0.5f && indexDown)
                    {
                        IndexRelease();
                        indexDown = false;
                    }

                    // Thumb Primary
                    if (OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) && !thumbDown)
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

    public void Dropped()
    {
        foreach (Collider c in toolCols)
        {
            if (!c.enabled)
                c.enabled = true;
        }
    }

    public void Grabbed()
    {
        LinesOff();
        toolCols = GetComponentsInChildren<Collider>();

        foreach (Collider c in toolCols)
        {
            if (!c.isTrigger && c.enabled)
                c.enabled = false;
        }
    }

    public void Release()
    {
        toolCols = GetComponentsInChildren<Collider>();

        foreach (Collider c in toolCols)
        {
            if (c.enabled && !c.isTrigger)
                c.enabled = false;
        }

        if (grabInfo != null)
            grabInfo.CancelGrab();
    }

    public bool isHeld()
    {
        if (grabInfo == null)
        {
            return false;
        }

        return grabInfo.isGrabbed;
    }

    void OnTriggerEnter(Collider col)
    {
        if (isHeld())
            return; 

        if ((col.CompareTag("LeftHand") || col.CompareTag("RightHand")))
        {
            LinesOn();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (isHeld())
            return;

        if ((col.CompareTag("LeftHand") || col.CompareTag("RightHand")))
        {
            LinesOff();
        }
    }

    void OnColliderEnter(Collider col)
    {
        OnTriggerEnter(col);
    }

    void OnColliderExit(Collider col)
    {
        OnTriggerExit(col);
    }

    public void LinesOn()
    {
        var renderers = GetComponentsInChildren<Renderer>();
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
