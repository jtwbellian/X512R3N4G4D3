using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hat : VRTool, iSpecial_Grabbable
{

    void iSpecial_Grabbable.OnGrab()
    {
    }

    void iSpecial_Grabbable.OnRelease()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        isHat = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void IndexTouch()
    {
        throw new System.NotImplementedException();
    }

    public override void IndexRelease()
    {
        throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        throw new System.NotImplementedException();
    }

    public override void ThumbRelease()
    {
        throw new System.NotImplementedException();
    }
}
