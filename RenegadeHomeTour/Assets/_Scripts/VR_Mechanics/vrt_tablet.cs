using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct PlayerData
{
    public IKPlayerController ikController;
    public VRMovementController movementController;
    public Transform head;

    public PlayerData(Transform root)
    {
        ikController = root.GetComponentInChildren<IKPlayerController>();
        movementController = root.GetComponentInChildren<VRMovementController>();
        head = Camera.main.transform;
        //Debug.Log("Player data = " + ikController);
    }
}

public class vrt_tablet : VRTool
{

    [SerializeField]
    private bool tutorialOn = true;
    [SerializeField]
    private Transform playerObj;
    private PlayerData playerData;
    private float distFromHead = 0.5f;
    private GameObject lastPage = null;

    //private int currentPage = 0;
    public Transform leftGrip, rightGrip, offsetTarget;
    public LiveCam selfiCam;
    public Text heightText;

    public GameObject[] pages;

    public override void IndexRelease(){}

    public override void IndexTouch(){}

    public override void Init()
    {
        Invoke("GetInFace", 1f);
        playerData = new PlayerData(playerObj);

        lastPage = pages[0];

        foreach (GameObject obj in pages)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        // Both thumbsticks down, call tablet
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            GetInFace();

        if (pages[1].activeSelf)
        {
            heightText.text = playerData.ikController.GetHeightStr();
        }

        if (tutorialOn)
        {
            var em = EventManager.GetInstance();
            if (em.currentEvent == 6 && !EventManager.sandboxMode)
            {
                if (Vector3.Distance(transform.position, Camera.main.transform.position) > distFromHead * 1.5f)
                {
                    Hud hud = GameManager.GetInstance().hud;
                    if (!hud.IsInvoking())
                        hud.ShowImage(Icon.analogClick, 2.5f);
                }
            }
            else if (EventManager.GetInstance().currentEvent > 4)
                tutorialOn = false;
        }

    }

    public void HeightAdd()
    {
        if (playerData.ikController.height >= playerData.ikController.MAX_HEIGHT)
            return;

        playerData.ikController.AdjustHeight(0.01f);
    }

    public void HeightSub()
    {
        if (playerData.ikController.height <= playerData.ikController.MIN_HEIGHT)
            return;

        playerData.ikController.AdjustHeight(-0.01f);
    }

    public void GetInFace()
    {
        if (grabInfo.isGrabbed)
        { grabInfo.grabbedBy.GrabEnd(); }

        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distFromHead;
        transform.LookAt(Camera.main.transform, Vector3.up );
        transform.Rotate(new Vector3(0,1.0f, 0), 180f);
        offsetTarget.localRotation = Quaternion.Euler(-45,0,0);
        offsetTarget.localPosition = Vector3.zero;
    }

    public override void ThumbRelease()
    {
    }

    public override void ThumbTouch()
    {
    }

    public override void OnGrab()
    {
        if (grabInfo == null)
            return;
            
        if (grabInfo.grabbedBy.CompareTag("RightHand"))
        {
            offsetTarget.SetParent(rightGrip);
            offsetTarget.localPosition = Vector3.zero;
        }
        else if (grabInfo.grabbedBy.CompareTag("LeftHand"))
        {
            offsetTarget.SetParent(leftGrip);
            offsetTarget.localPosition = Vector3.zero;
        }
        base.OnGrab();

        LiveCam.SetActive(selfiCam);

        if (lastPage != null)
            lastPage.SetActive(true);
    }

    public override void OnRelease()
    {
        foreach (GameObject obj in pages)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
                lastPage = obj;
            }
        }
        offsetTarget.SetParent(transform);
        //transform.localPosition = Vector3.zero;
        base.OnRelease();
    }

}
