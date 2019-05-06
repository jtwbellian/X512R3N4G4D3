using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LiveCam : MonoBehaviour
{
    public static LiveCam activeCam = null;
    [HideInInspector]
    public Camera cam = null;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void OnTriggerEnter(Collider col)
    {
        LiveCam.SetActive(this);
    }
    private void OnTriggerExit(Collider col)
    {
        LiveCam.Clear();
    }

    public static void SetActive(LiveCam liveCam)
    {
        if (activeCam != null)
        {
            activeCam.cam.enabled = false;
        }

        activeCam = liveCam;
        liveCam.cam.enabled = true;
    }

    public static void Clear()
    {
        if (LiveCam.activeCam == null)
        {
            Debug.Log("LiveCam cleared, but no LiveCams on");
            return;
        }

        LiveCam.activeCam.cam.enabled = false;
        LiveCam.activeCam = null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
