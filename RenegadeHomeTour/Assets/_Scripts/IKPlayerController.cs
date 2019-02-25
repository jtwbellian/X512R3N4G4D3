using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPlayerController : MonoBehaviour
{
    private const float MAX_FOOT_HEIGHT = -3f;
    private const float DEFAULT_HEIGHT = 1.7f;
    private const float MAX_RAYDIST = 25f;


    private bool crouch = false;
    private float lGrab = 0f;
    private float lFinger = 0f;
    private float lThumb = 0f;
    private float rGrab = 0f;
    private float rFinger = 0f;
    private float rThumb = 0f;
    private Transform []  feet;
    private int numFeet = 0;

    private Transform handR;
    private Transform handL;
    private Collider fistR;
    private Collider fistL;

    public Transform head;
    public float height = 1.7f;
    public bool leftyMode = false;
    public float offset = 0.0f;

    private float turningRate = 1f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.Log("Error: Animator component not found");
        }


        for (int i = 0; i < 8; i ++)
            animator.SetLayerWeight(i, 1);

        handL = GameObject.FindWithTag("LeftHand").transform;
        handR = GameObject.FindWithTag("RightHand").transform;

        fistL = handL.GetComponent<Collider>();
        fistR = handR.GetComponent<Collider>();
    }

    public void UpdatePlayerHeight()
    {
        height = head.localPosition.y;
        float newScale = height / DEFAULT_HEIGHT;

        transform.localScale = new Vector3(newScale, newScale, newScale);
        GetComponent<CapsuleCollider>().height = newScale;
    }


    // Updates the values for hand positions based on Oculus Input
    void UpdateGestures()
    {        
        rGrab = (leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        rFinger = (leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        rThumb = (leftyMode) ? (OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) ? 1f:0f) : (OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons) ? 0f : 1f);

        lGrab = (!leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        lFinger = (!leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        lThumb = (!leftyMode) ? (OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) ? 1f : 0f) : (OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons) ? 0f : 1f);

        animator.SetFloat("RGrab", rGrab);
        animator.SetFloat("RFinger", rFinger);
        animator.SetFloat("RThumb", rThumb);

        animator.SetFloat("LGrab", lGrab);
        animator.SetFloat("LFinger", lFinger);
        animator.SetFloat("LThumb", lThumb);
    }


    // Check Pointing
    void CheckPointing()
    {
        /*
        int layerMask = LayerMask.GetMask("Interactable");
        RaycastHit hit;

        // RayCast for interactables from Left Hand
        if (lThumb == 0f && lFinger == 0f)
        {

            if (Physics.Raycast(handL.position,handL.forward, out hit, MAX_RAYDIST, layerMask))
            {
                VRTool item = hit.transform.GetComponent<VRTool>();

                if (item != null)
                {
                    item.LinesOn();

                    if (lGrab > 0.5f)
                        item.transform.position = handL.position;
                }
            }
        }

        // RayCast for interactables from Right Hand
        if (rGrab > 0.5f && rThumb == 0f && rFinger == 0f)
        {

            if (Physics.Raycast(handR.position, handR.forward, out hit, MAX_RAYDIST, layerMask))
            {
                VRTool item = hit.transform.GetComponent<VRTool>();

                if (item != null)
                {
                    item.LinesOn();

                    if (lGrab > 0.5f)
                        item.transform.position = handR.position;
                }
            }
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        // if head lower than 3/4 height
        if (head.localPosition.y < height * 0.75f && !crouch)
        {
            animator.SetBool("LegsUp", true);
            crouch = true;
            Debug.Log("Crouch");
            GetComponent<CapsuleCollider>().height = height / 2f;
        }

        if (crouch && head.localPosition.y > height *0.9)
        {
            Debug.Log("unCrouch");
            animator.SetBool("LegsUp", false);
            crouch = false;
            GetComponent<CapsuleCollider>().height = height;
        }

        // position the players body
        if (transform.position !=  head.position)
        {
            transform.position = new Vector3(head.position.x, head.position.y - height, head.position.z) + head.transform.forward * offset;
        }

        // Turn on Left fist
        if (lGrab > 0.5f && !fistL.isTrigger && !handL.GetComponent<OVRGrabber>().grabbedObject)
        {
            fistL.isTrigger = true;
        }
        else if (fistL.isTrigger)
        {
            fistL.isTrigger = false;
        }

        // Turn on Right fist
        if (rGrab > 0.5f && !fistR.isTrigger && !handR.GetComponent<OVRGrabber>().grabbedObject)
        {
            fistR.isTrigger = true;
        }
        else if (fistR.isTrigger)
        {
            fistR.isTrigger = false;
        }

        UpdateGestures();

        //CheckPointing();

        // Click both sticks in to reset height and scale
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        { 
            UpdatePlayerHeight();
        }

    }

}
