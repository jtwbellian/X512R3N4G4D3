using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPlayerController : EVActor
{

    private const float MAX_FOOT_HEIGHT = -3f;
    private const float DEFAULT_HEIGHT = 1.6f;
    private const float MAX_RAYDIST = 25f;
    //private float minHandRadius = 0.01f;
    //private float maxHandRadius = 0.05f

    private float lGrab = 0f;
    private float lFinger = 0f;
    private float lThumb = 0f;
    private float rGrab = 0f;
    private float rFinger = 0f;
    private float rThumb = 0f;
    [Header("IK Options")]
    public bool ikOn = false;

    [Tooltip("Meshes and skeleton associated with IK but NOT the IK Controller itself")]
    public GameObject [] parts_ik;
    [Tooltip("Must be left hand followed by right hand")]
    public GameObject[] parts_nonIk;

    public Transform handR;
    public Transform handL;

    public CapsuleCollider capsule;

    public Transform head;
    public float height = 1.7f;
    public bool leftyMode = false;
    public float offset = 0.0f;

    private Animator BodyAnimator;
    private Animator LHandAnimator;
    private Animator RHandAnimator;


    public void IKOn()
    {
        ikOn = true;
        RefreshIKMode();
    }

    public void IKOff()
    {
        ikOn = false;
        RefreshIKMode();
    }

    private void RefreshIKMode()
    { 
        foreach (GameObject obj in parts_ik)
        {
            obj.SetActive(ikOn);
        }

        foreach (GameObject obj in parts_nonIk)
        {
            obj.SetActive(!ikOn);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        subscribesTo = AppliesTo.PLAYER;

        BodyAnimator = GetComponent<Animator>();
        LHandAnimator = parts_nonIk[0].GetComponent<Animator>();
        RHandAnimator = parts_nonIk[1].GetComponent<Animator>();

        if (BodyAnimator == null)
        {
            Debug.Log("Error: Animator component not found");
        }

        for (int i = 0; i < 9; i++)
        {
            BodyAnimator.SetLayerWeight(i, 1);
            LHandAnimator.SetLayerWeight(i, 1);
            RHandAnimator.SetLayerWeight(i, 1);
        }

        BodyAnimator.speed = 1f;
        LHandAnimator.speed = 1f;
        RHandAnimator.speed = 1f;

        //Physics.IgnoreCollision(capsule, head.transform.GetComponent<Collider>());

        // Find all Passable objects and set physics to ignore collisions between them and the player
        GameObject [] passableObjs =  GameObject.FindGameObjectsWithTag("passable");

        for(var i = 0; i < passableObjs.Length; i ++)
        {
            var colliders = passableObjs[i].GetComponentsInChildren<Collider>();

            foreach (Collider c in colliders)
            {
                Physics.IgnoreCollision(capsule, c);
            }
        }
        RefreshIKMode();
       }

    public void UpdatePlayerHeight()
    {
        OVRManager.display.RecenterPose();
        height = head.localPosition.y;
        float newScale = height / DEFAULT_HEIGHT;

        var scale = new Vector3(newScale, newScale, newScale);

        transform.localScale = scale;
        LHandAnimator.transform.localScale = scale;
        RHandAnimator.transform.localScale = scale;
        //capsule.height = newScale;

        if (myEvent.type == EV.Calibrated)
        {
            EventManager.CompleteTask(this);
        }

        GameManager gm = GameManager.GetInstance();
        gm.hud.ShowImage(Icon.calibrate, 2f);
    }


    // Updates the values for hand positions based on Oculus Input
    void UpdateGestures()
    {
        rGrab = (leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        rFinger = (leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        rThumb = (leftyMode) ? (OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) ? 1f : 0f) : (OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons) ? 0f : 1f);

        lGrab = (!leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        lFinger = (!leftyMode) ? OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) : OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        lThumb = (!leftyMode) ? (OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) ? 1f : 0f) : (OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons) ? 0f : 1f);

        if (ikOn)
        {
            BodyAnimator.SetFloat("RGrab", rGrab);
            BodyAnimator.SetFloat("RFinger", rFinger);
            BodyAnimator.SetFloat("RThumb", rThumb);

            BodyAnimator.SetFloat("LGrab", lGrab);
            BodyAnimator.SetFloat("LFinger", lFinger);
            BodyAnimator.SetFloat("LThumb", lThumb);
        }
        else
        {
            RHandAnimator.SetFloat("RGrab", rGrab);
            RHandAnimator.SetFloat("RFinger", rFinger);
            RHandAnimator.SetFloat("RThumb", rThumb);

            LHandAnimator.SetFloat("LGrab", lGrab);
            LHandAnimator.SetFloat("LFinger", lFinger);
            LHandAnimator.SetFloat("LThumb", lThumb);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make collider match your current height

        var scaleFactor = 6f;
        var minHeight = 0.2f;
        var percentHeight = 0.5f;
 
        capsule.height = (Mathf.Abs(head.localPosition.y) * percentHeight + minHeight) * scaleFactor;

        capsule.transform.localPosition = new Vector3(head.localPosition.x, head.localPosition.y - height/2, head.localPosition.z);

        // position the players body
        if (transform.position !=  head.position)
        {
            transform.position = new Vector3(head.position.x, head.position.y - height, head.position.z) + head.transform.forward * offset;
        }

        UpdateGestures();

        //CheckPointing();

        // Click both sticks in to reset height and scale
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            //GameManager.GetInstance().direc.Ping(PING.calibrated);
            UpdatePlayerHeight();
        }

        // Press A + X for menu
        if (OVRInput.Get(OVRInput.Button.One) && OVRInput.Get(OVRInput.Button.Three))
        {
            GameManager.GetInstance().hud.ToggleMenu();
        }

        var lerp = (head.localPosition.y - 0.75f) * 2f / height;

        BodyAnimator.SetFloat("Legs", lerp); //Mathf.Clamp(head.localPosition.y / (height * 0.75f), 0f, 1f));
    }
}
