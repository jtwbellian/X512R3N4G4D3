using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum VibrationForce
{
    Light,
    Medium,
    Hard,
}

public class OVRHapticsManager : MonoBehaviour
{
    [SerializeField]
    OVRInput.Controller controllerMask;
    public static OVRHapticsManager instance = null;
    public OVRGrabber handR;
    public OVRGrabber handL;

    private OVRHapticsClip clipLight;
    private OVRHapticsClip clipMedium;
    private OVRHapticsClip clipHard;


    void Awake()
    {
        if (OVRHapticsManager.instance == this)
            return;

        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
            instance = this;
        }

        InitializeOVRHaptics();
    }

    public static OVRHapticsManager GetInstance()
    {
        return instance;
    }

    private void InitializeOVRHaptics()
    {

        int cnt = 10;
        clipLight = new OVRHapticsClip(cnt);
        clipMedium = new OVRHapticsClip(cnt);
        clipHard = new OVRHapticsClip(cnt);

        var cl = clipLight.Samples;
        System.Array.Resize<byte>(ref cl, cnt);

        var cm = clipLight.Samples;
        System.Array.Resize<byte>(ref cm, cnt);

        var ch = clipLight.Samples;
        System.Array.Resize<byte>(ref ch, cnt);

        for (int i = 0; i < clipLight.Samples.Length; i++)
        {
            clipLight.Samples.SetValue(i % 2 == 0 ? (byte)0 : (byte)45, i);
            clipMedium.Samples.SetValue(i % 2 == 0 ? (byte)0 : (byte)100, i);
            clipHard.Samples.SetValue(i % 2 == 0 ? (byte)0 : (byte)180, i);
        }

        clipLight = new OVRHapticsClip(clipLight.Samples, clipLight.Samples.Length);
        clipMedium = new OVRHapticsClip(clipMedium.Samples, clipMedium.Samples.Length);
        clipHard = new OVRHapticsClip(clipHard.Samples, clipHard.Samples.Length);
    }

    public void Play(VibrationForce force, OVRInput.Controller hand, float time)
    {
        StartCoroutine(VibrateTime(force,hand,time));
    }

    public void BuzzRight(VibrationForce force, float time)
    {
        Play(force, handR.m_controller, time);
    }

    public void BuzzLeft(VibrationForce force, float time)
    {
        Play(force, handL.m_controller, time);
    }

    public IEnumerator VibrateTime(VibrationForce force, OVRInput.Controller hand, float time)
    {
        //Debug.Log("HapticsCalled");
        var forcedHaptic = true;

        var channel = OVRHaptics.RightChannel;

        if (hand == OVRInput.Controller.LTouch)
            channel = OVRHaptics.LeftChannel;

        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            switch (force)
            {
                case VibrationForce.Light:
                    channel.Queue(clipLight);
                    break;
                case VibrationForce.Medium:
                    channel.Queue(clipMedium);
                    break;
                case VibrationForce.Hard:
                    channel.Queue(clipHard);
                    break;
            }
        }

        yield return new WaitForSeconds(time);
        channel.Clear();
        forcedHaptic = false;
        yield return null;

    }
}
