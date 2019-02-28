using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class VRTool : MonoBehaviour, iSpecial_Grabbable
{
    private Rigidbody rb;
    private Collider[] toolCols;
    protected OVRGrabbable grabInfo;
    private Renderer [] renderers;

    private bool indexDown = false;
    private bool thumbDown = false;

    public bool usesIndex = true;
    public bool usesThumb = false;

    public bool isHat = false;
    public float indexValue = 0f;
    public GrabMagnet home;
    public OVRHapticsManager haptics;

    [HideInInspector]
    public int hand; // 0 = primary 1 = secondary


    // Start is called before the first frame update
    void Start()
    {
        toolCols = GetComponentsInChildren<Collider>();
        grabInfo = GetComponent<OVRGrabbable>();
        rb = GetComponentInChildren<Rigidbody>();
        renderers = GetComponentsInChildren<Renderer>();

        Init();
    }

    public OVRGrabbable GetGrabber()
    {
        return grabInfo;
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

    public bool isHeld()
    {
        if (grabInfo == null)
        {
            return false;
        }

        return grabInfo.isGrabbed;
    }

    public Rigidbody GetRB()
    {
        return rb;
    }

    void OnTriggerEnter(Collider col)
    {
        if (isHeld())
            return; 

        if ((col.CompareTag("LeftHand") || col.CompareTag("RightHand")))
        {
            LinesOn();
            haptics.Play(VibrationForce.Light, grabInfo.grabbedBy.m_controller, 1f);

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

    public Vector3 GetVelocity()
    {
        var grabHand = grabInfo.grabbedBy;

        if (grabHand == null)
            return Vector3.zero;

        return grabHand.GetHandVelocity();
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
        renderers = GetComponentsInChildren<Renderer>();
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

    public void SetHome(GrabMagnet grabSpot)
    {
        home = grabSpot;
        transform.parent = grabSpot.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        foreach (Collider c in toolCols)
        {
            if (!c.isTrigger && c.enabled)
                c.enabled = false;
        }

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        //Debug.Log("Home Set to " + grabSpot.ToString());
    }

    public void GoHome()
    {
        if (home == null || grabInfo.isGrabbed)
            return;

       // Debug.Log("Attempting to send " + this.ToString() + " To its home @ " + home.ToString());

        if (home.IsFree())
        {
            transform.position = home.transform.position;
        }
        else
        {
            //Debug.Log("Home Full");
            home = null;
        }

    }


    // Implement iSpecial Grabbable

    public void OnGrab()
    {
        Debug.Log(this.ToString() + " Grabbed");

        if (home != null)
        {
            Debug.Log("Home: " + home.ToString() + "\n Attempting to free..." );
            home.Free();
        }

        LinesOff();
        toolCols = GetComponentsInChildren<Collider>();

        foreach (Collider c in toolCols)
        {
            if (!c.isTrigger && c.enabled)
                c.enabled = false;
        }

        rb.isKinematic = true;
    }

    public void OnRelease()
    {
        Debug.Log(this.ToString() + " Released");

        toolCols = GetComponentsInChildren<Collider>();

        foreach (Collider c in toolCols)
        {
            if (!c.enabled && !c.isTrigger)
                c.enabled = true;
        }

        if (rb != null)
            rb.isKinematic = false;

        if (grabInfo != null)
            grabInfo.CancelGrab();

        if (home != null)
        {
            Invoke("GoHome", 10f);
        }
    }
}
