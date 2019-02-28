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


    private OVRHapticsClip clipLight;
    private OVRHapticsClip clipMedium;
    private OVRHapticsClip clipHard;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitializeOVRHaptics();

    }

    private void InitializeOVRHaptics()
    {

        int cnt = 10;
        clipLight = new OVRHapticsClip(cnt);
        clipMedium = new OVRHapticsClip(cnt);
        clipHard = new OVRHapticsClip(cnt);
        for (int i = 0; i < cnt; i++)
        {
            clipLight.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)45;
            clipMedium.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)100;
            clipHard.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)180;
        }

        clipLight = new OVRHapticsClip(clipLight.Samples, clipLight.Samples.Length);
        clipMedium = new OVRHapticsClip(clipMedium.Samples, clipMedium.Samples.Length);
        clipHard = new OVRHapticsClip(clipHard.Samples, clipHard.Samples.Length);
    }

    public void Play(VibrationForce force, OVRInput.Controller hand, float time)
    {
        StartCoroutine(VibrateTime(force,hand,time));
    }

    public IEnumerator VibrateTime(VibrationForce force, OVRInput.Controller hand, float time)
    {
        Debug.Log("HapticsCalled");
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
