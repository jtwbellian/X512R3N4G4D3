using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hat : VRTool, iSpecial_Grabbable
{
    private SphereCollider myCollider;

    void iSpecial_Grabbable.OnGrab()
    {
    }

    void iSpecial_Grabbable.OnRelease()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        isHat = true;
        myCollider = GetComponent<SphereCollider>();
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
