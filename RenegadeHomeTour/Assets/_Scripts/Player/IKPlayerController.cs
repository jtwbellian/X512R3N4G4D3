using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPlayerController : EVActor
{
    private const float MAX_FOOT_HEIGHT = -3f;
    private const float DEFAULT_HEIGHT = 1.683f;
    private const float MAX_RAYDIST = 25f;
    private const float CHAIR_SCALE_FACTOR = 1.336f;

    // avg distance from eyes to Forehead
    public const float E2F = 0.08f; //0.127f;
    //private float minHandRadius = 0.01f;
    //private float maxHandRadius = 0.05f
    private float legLerp = 0f;
    private bool has_been_calibrated = false;
    private float lGrab = 0f;
    private float lFinger = 0f;
    private float lThumb = 0f;
    private float rGrab = 0f;
    private float rFinger = 0f;
    private float rThumb = 0f;

    private float scaleFactor = 4f;
    private float minHeight = 0.2f;
    private float percentHeight = 0.5f;
    [SerializeField]
    private float headOffset = 0.053f;

    public float MAX_HEIGHT = 2.134f;
    public float MIN_HEIGHT = 1.067f;

    [Header("IK Options")]
    public bool ikOn = false;

    [Tooltip("Must be left hand followed by right hand")]
    public GameObject [] pointers;
    [Tooltip("Meshes and skeleton associated with IK but NOT the IK Controller itself")]
    public GameObject [] parts_ik;
    public Collider[] ikCols;
    [Tooltip("Must be left hand followed by right hand")]
    public GameObject[] parts_nonIk;
    public Collider[] nonIkCols;

    public Transform handR;
    public Transform handL;

    public CapsuleCollider capsule;

    public Transform head;
    public float height = 1.7f;
    public bool leftyMode = false;
    public bool chairMode = false;
    public float offset = 0.0f;

    private Animator BodyAnimator;
    private Animator LHandAnimator;
    private Animator RHandAnimator;
    private Animator NonIKBodyAnimator;

    private GestureHandler  gestureHandler;

    #region toggles
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

    public void ChairModeOn()
    {
        chairMode = true;
        UpdatePlayerHeight();
    }
    public void ChairModeOff()
    {
        chairMode = false;
        UpdatePlayerHeight();
    }
    #endregion

    public void FreePlayer()
    {
        if (myEvent == null)
            return;

        if (myEvent.type == EV.analogFwd)
        {
            var mc = transform.root.GetComponent<VRMovementController>();
            mc.AllowBoost();
            CompleteEvent();
            has_been_calibrated = true;
        }
    }

    [ContextMenu("Refresh_IK_Mode")]
     private void RefreshIKMode()
    { 
        var tools = GetComponentsInChildren<VRTool>();

        foreach (var t in tools)
        {
            t.OnRelease();
            t.transform.SetParent(null);
        }

        foreach (GameObject obj in parts_ik)
        {
            obj.SetActive(ikOn);
        }

        foreach (GameObject obj in parts_nonIk)
        {
            obj.SetActive(!ikOn);
        }

        var gm = GameManager.GetInstance();

        if (gm != null)
        {
            if (ikOn)
                gm.UpdatePlayerCols(ikCols);
            else
                gm.UpdatePlayerCols(nonIkCols);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        subscribesTo = AppliesTo.PLAYER;

        BodyAnimator = GetComponent<Animator>();
        LHandAnimator = parts_nonIk[0].GetComponent<Animator>();
        RHandAnimator = parts_nonIk[1].GetComponent<Animator>();
        NonIKBodyAnimator = parts_nonIk[2].GetComponent<Animator>();

        gestureHandler = GetComponent<GestureHandler>();

        if (BodyAnimator == null)
        {
            Debug.Log("Error: Animator component not found");
        }

        /*
        NonIKBodyAnimator.speed = 1f;
        BodyAnimator.speed = 1f;
        LHandAnimator.speed = 1f;
        RHandAnimator.speed = 1f;


        NonIKBodyAnimator.SetLayerWeight(0, 1);

        for (int i = 0; i < 8; i++)
        {
            if (i < 4)
            {
                LHandAnimator.SetLayerWeight(i, 1);
                RHandAnimator.SetLayerWeight(i, 1);
            }
            BodyAnimator.SetLayerWeight(i, 1);
        }
        BodyAnimator.SetLayerWeight(8, 1);
        */

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
        NonIKBodyAnimator.gameObject.SetActive(false);
       }


    public void AdjustHeight(float amt)
    {
        height += amt;

        float newScale = Mathf.Clamp(height, MIN_HEIGHT, MAX_HEIGHT) / DEFAULT_HEIGHT;
        var scale = new Vector3(newScale, newScale, newScale);

        if (ikOn)
            transform.localScale = scale;
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            LHandAnimator.transform.localScale = scale;
            RHandAnimator.transform.localScale = scale;
            NonIKBodyAnimator.transform.localScale = scale;
        }
    }

    public void UpdatePlayerHeight()
    {
        OVRManager.display.RecenterPose();
        height = head.localPosition.y;

        if (chairMode)
        {
            height *= CHAIR_SCALE_FACTOR;
        }

        float newScale = Mathf.Clamp(height, MIN_HEIGHT, MAX_HEIGHT) / DEFAULT_HEIGHT;
 
        var scale = new Vector3(newScale, newScale, newScale);

        if (ikOn)
            transform.localScale = scale;
        else
        {
            transform.localScale = new Vector3(1f,1f,1f);
            LHandAnimator.transform.localScale = scale;
            RHandAnimator.transform.localScale = scale;
            NonIKBodyAnimator.transform.localScale = scale;
        }
        //capsule.height = newScale;

        GameManager gm = GameManager.GetInstance();
        gm.hud.ShowImage(Icon.calibrate, 2f);

        if (myEvent == null)
            return;

        if (myEvent.type == EV.Calibrated)
        {
            EventManager.CompleteTask(this);
        }
    }

    public string GetHeightStr()
    { 
        var hs = height + E2F;
        return hs.ToString("F2") + "m";
    }
    
    // Check basic Gestures
    public bool OpenHand(bool lefty)
    {
        if (lefty && lGrab < 0.5f && lFinger < 0.5f)
        {
            return true;
        }
        if (rGrab < 0.5f && rFinger < 0.5f)
        {
            return true;
        }
        return false;
    }

    // Updates the values for hand positions based on Oculus Input
    void UpdateGestures()
    {
        rGrab = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        rFinger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        rThumb = OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons) ? 0f : 1f;

        lGrab = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        lFinger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        lThumb = OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons) ? 1f : 0f;

        if (lGrab > 0.5f && pointers[0].activeSelf)
            pointers[0].SetActive(false);
        else if (lGrab < 0.5f && !pointers[0].activeSelf)
            pointers[0].SetActive(true);

        if (rGrab > 0.5 && pointers[1].activeSelf)
            pointers[1].SetActive(false);
        else if (rGrab < 0.5 && !pointers[1].activeSelf)
            pointers[1].SetActive(true);

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
        if (GameManager.isPaused)
        {
            if (!ikOn)
            {
                foreach (GameObject g in parts_nonIk)
                {
                    g.SetActive(false);
                }
            }
            else if (ikOn)
            {
                foreach (GameObject g in parts_ik)
                {
                    g.SetActive(false);
                }
            }
            return;
        }
        else if (!parts_nonIk[0].activeInHierarchy || !parts_ik[0].activeInHierarchy)
        {
            if (!ikOn)
            {
                for (int i = 0; i < parts_nonIk.Length; i++)
                {
                    if (i == 2 && !has_been_calibrated)
                        continue;
                    parts_nonIk[i].SetActive(true);
                }
            }
            else if (ikOn)
            {
                foreach (GameObject g in parts_ik)
                {
                    g.SetActive(true);
                }
            }
        }


        // make collider match your current height
        if (ikOn)
            capsule.height = (Mathf.Abs(head.localPosition.y) * percentHeight + minHeight) * scaleFactor;
        else
            capsule.height = (Mathf.Abs(head.localPosition.y * CHAIR_SCALE_FACTOR) * percentHeight + minHeight) * scaleFactor;

        capsule.transform.localPosition = new Vector3(head.localPosition.x, head.localPosition.y - height/2, head.localPosition.z);

        // position the players body
        if (ikOn)
        {
            if (transform.position != head.position)
            {
                transform.position = new Vector3(head.position.x, head.position.y - height, head.position.z) + head.transform.forward * offset;
            }
        }
        else
        {
            if (NonIKBodyAnimator.transform.position != head.position)
            {
                NonIKBodyAnimator.transform.position = new Vector3(0, 0, height - headOffset);
            }
        }

        UpdateGestures();

        //CheckPointing();

        // Click both sticks in to reset height and scale
        /*
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            //GameManager.GetInstance().direc.Ping(PING.calibrated);
            UpdatePlayerHeight();
        }*/

        // Press A + X for menu
       // if (OVRInput.Get(OVRInput.Button.One) && OVRInput.Get(OVRInput.Button.Three))
       // {
       //     GameManager.GetInstance().hud.ToggleMenu();
      //  }

        legLerp = (head.localPosition.y - height / 2) / (height / 2);

        if (ikOn)
            BodyAnimator.SetFloat("Legs", legLerp);
        else
           NonIKBodyAnimator.SetFloat("LegsUp", legLerp);
    }

    public override void BeginEvent()
    {}
}
