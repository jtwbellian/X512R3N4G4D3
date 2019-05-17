using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hat : VRTool
{
    public bool isOutfit = false;
    public bool hidden = false;
    MeshRenderer myMesh;

    public override void Init()
    {
        isHat = true;
        myMesh = GetComponentInChildren<MeshRenderer>();
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

    public override void OnGrab()
    {
        if (hidden)
        {
            OutfitOff();
        }
        base.OnGrab();
    }

   /* public override void SetHome(GrabMagnet grabSpot)
    {
        base.SetHome(grabSpot);

    }
    public override void OnRelease()
    {
        base.OnRelease();
    }*/

    public void OutfitOn()
    {
        Debug.Log("Hat On");
        var myMat = myMesh.materials[1];
        GameManager.GetInstance().ChangeSkin(myMat);
        myMesh.enabled = false;
        hidden = true;
    }

    public void OutfitOff()
    {
        GameManager gm = GameManager.GetInstance();

        Debug.Log("Hat Off");
        //SkinnedMeshRenderer smr = home.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
       // MeshRenderer mr = Camera.main.GetComponentInChildren<MeshRenderer>();

        myMesh.enabled = true;
        hidden = false;
        GameManager.GetInstance().ChangeSkin(gm.defaultPlayerMat);
    }


}
