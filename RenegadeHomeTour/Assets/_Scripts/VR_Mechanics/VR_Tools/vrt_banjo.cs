
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_banjo : VRTool
{
    private DoesDammage dd;
    private OVRHapticsManager hm;
    private VRKey strings;

    public AudioClip [] chords;


    // Start is called before the first frame update
    public override void Init()
    {
        hm = OVRHapticsManager.instance;
        dd = GetComponentInChildren<DoesDammage>();
        strings = GetComponentInChildren<VRKey>();
    }

    public override void IndexRelease()
    {
        if (chords.Length > 0)
            strings.clip = chords[0];
    }

    public override void IndexTouch()
    {
        if (chords.Length > 1)
            strings.clip = chords[1];
    }

    public void OnGrab()
    {
        base.OnGrab();

        if (grabInfo.grabbedBy != null)
            dd.Enable();
    }

    public void OnRelease()
    {
        base.OnRelease();
        dd.Disable();
    }

    public override void ThumbRelease()
    {
        //throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        //throw new System.NotImplementedException();
    }

}
