using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPlayerController : MonoBehaviour
{
    private const float MAX_FOOT_HEIGHT = -3f;
    private const float DEFAULT_HEIGHT = 1.7f;

    private float lGrab = 0f;
    private float lFinger = 0f;
    private float lThumb = 0f;
    private float rGrab = 0f;
    private float rFinger = 0f;
    private float rThumb = 0f;
    private Transform []  feet;
    private int numFeet = 0;

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


        for (int i = 0; i < 7; i ++)
            animator.SetLayerWeight(i, 1);
    }

    public void UpdatePlayerHeight()
    {
        var height = head.localPosition.y;
        float newScale = height / DEFAULT_HEIGHT;

        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    void UpdateGestures()
    {

        // Click both sticks in to reset height and scale
        if(OVRInput.Get(OVRInput.Button.SecondaryThumbstick) && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            Debug.Log("heightReset");
            UpdatePlayerHeight();
        }
        
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

    // Update is called once per frame
    void Update()
    {
        if (transform.position !=  head.position)
        {
            transform.position = new Vector3(head.position.x, head.position.y - height, head.position.z) + head.transform.forward * offset;
        }

        UpdateGestures();
        //CalcFeet();
        // Turn towards our target rotation
        //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
    }
}
