using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hat : VRTool
{
    public bool isOutfit = false;
    public bool hidden = false;

    public override void Init()
    {
        isHat = true;
    }

    public override void IndexRelease()
    {
    }

    public override void IndexTouch()
    {


    }


    public override void ThumbRelease()
    {
        //throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGrab()
    {
        base.OnGrab();

        if (hidden)
        {
            SkinnedMeshRenderer smr = Camera.main.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
            MeshRenderer mr = GetComponent<MeshRenderer>();
            smr.material = GameManager.GetInstance().defaultPlayerMat;
            mr.enabled = true;
            hidden = false;
        }

    }
    public void SetHome(GrabMagnet grabSpot)
    {
        base.SetHome(grabSpot);

    }
    public void OnRelease()
    {
        base.OnRelease();
    }

    public void OutfitOn()
    {
        Debug.Log("Hat changed");
        SkinnedMeshRenderer smr = home.transform.root.GetComponent<SkinnedMeshRenderer>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        smr.material = mr.materials[1];
        mr.enabled = false;
        hidden = true;
    }

}
