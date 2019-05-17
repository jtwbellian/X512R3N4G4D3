using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class VRTool : iSpecial_Grabbable
{
    private Rigidbody rb;

    public OVRGrabbable grabInfo;

    private Renderer [] renderers;

    private float indexValue = 0f;
    private bool indexDown = false;
    private bool thumbDown = false;

    [HideInInspector]
    public Collider[] toolCols;

    public bool usesIndex = true;
    public bool usesThumb = false;

    public bool isHat = false;
    public bool isTool = true;

    public GrabMagnet home;
    public OVRHapticsManager haptics;

    [HideInInspector]
    public int hand; // 0 = primary 1 = secondary

    // Start is called before the first frame update
    void Start()
    {
        grabInfo = GetComponent<OVRGrabbable>();
        toolCols = grabInfo.allColliders;
        rb = GetComponentInChildren<Rigidbody>();
        renderers = GetComponentsInChildren<Renderer>();

        Grab += OnGrab;
        Release += OnRelease;

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
            //haptics.Play(VibrationForce.Light, GetGrabber().grabbedBy.m_controller, 1f);
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
        {
            //Debug.Log("GrabHand not found");
            return Vector3.zero;
        }

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
        renderers = GetComponentsInChildren<Renderer>();
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

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    public void GoHome()
    {
        if (home == null || grabInfo.isGrabbed)
            return;

        if (home.IsFree())
        {
            transform.position = home.transform.position;
        }
        else
        {
            home = null;
        }

    }

    // Implement iSpecial Grabbable
    public virtual void OnGrab()
    {
        if (home != null)
        {
            //Debug.Log("Home: " + home.ToString() + "\n Attempting to free..." );
            home.Free();
        }

        LinesOff();
        toolCols = GetComponentsInChildren<Collider>();

        foreach (Collider c in toolCols)
        {
            // Shrink sphere collider on grab
            /*
            if (c.isTrigger && c.GetType() == typeof(SphereCollider))
            {
                SphereCollider sc = (SphereCollider)c;
                sc.radius = minRadius;
            }
            else */

            if (!c.isTrigger)
            {
                //c.isTrigger = true;
                Collider [] playerCols = GameManager.GetInstance().playerCols;
                
                foreach (Collider playerCol in playerCols)
                    Physics.IgnoreCollision(c, playerCol);
            }
        }
        rb.isKinematic = true;
    }

    public virtual void OnRelease()
    {
        toolCols = GetComponentsInChildren<Collider>();

        /*
        foreach (Collider c in toolCols)
        {
            // Grow sphere collider on release
            if (c.isTrigger && c.GetType() == typeof(SphereCollider))
            {
                SphereCollider sc = (SphereCollider)c;
                sc.radius = maxRadius;
            }
            /*else if (c.GetType() != typeof(SphereCollider))
            {
                var playerBody = GameManager.GetInstance().playerBody;
                Physics.IgnoreCollision(c, playerBody, false);
                //c.isTrigger = false;
            }*/
        //}

        if (rb != null)
            rb.isKinematic = false;

        if (grabInfo != null)
            grabInfo.CancelGrab();

        if (home != null)
        {
            Invoke("GoHome", 15f);
        }
    }
}
