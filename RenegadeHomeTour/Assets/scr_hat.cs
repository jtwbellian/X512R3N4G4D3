using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hat : VRTool
{


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
    }

    public void OnRelease()
    {
        base.OnRelease();
    }
}
