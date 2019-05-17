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
    private Transform playerObj;
    private PlayerData playerData;
    private float distFromHead = 0.5f;
    //private int currentPage = 0;
    public Transform leftGrip, rightGrip, offsetTarget;
    public LiveCam selfiCam;
    public Text heightText;

    public GameObject[] pages;

    public override void IndexRelease()
    {
    }

    public override void IndexTouch()
    {
    }

    public override void Init()
    {
        Invoke("GetInFace", 1f);
        playerData = new PlayerData(playerObj);

        foreach (GameObject obj in pages)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (pages[1].activeSelf)
        {
            heightText.text = playerData.ikController.GetHeightStr();
        }
    }

    public void HeightAdd()
    {
        playerData.ikController.AdjustHeight(0.01f);
    }
    public void HeightSub()
    {
        playerData.ikController.AdjustHeight(-0.01f);
    }

    public void GetInFace()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distFromHead;
        transform.LookAt(Camera.main.transform, Vector3.up );
        transform.Rotate(new Vector3(0,1.0f, 0), 180f);
        offsetTarget.localRotation = Quaternion.Euler(-45,0,0);
    }

    public override void ThumbRelease()
    {
    }

    public override void ThumbTouch()
    {
    }

    public override void OnGrab()
    {
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

        if (pages != null)
            pages[0].SetActive(true);
    }

    public override void OnRelease()
    {
        foreach (GameObject obj in pages)
        {
            obj.SetActive(false);
        }

        offsetTarget.SetParent(transform);
        //transform.localPosition = Vector3.zero;
        base.OnRelease();
    }

}
