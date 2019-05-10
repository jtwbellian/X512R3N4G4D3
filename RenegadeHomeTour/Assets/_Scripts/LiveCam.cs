using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LiveCam : MonoBehaviour
{
    public static LiveCam activeCam = null;
    [HideInInspector]
    public Camera cam = null;
    public MeshRenderer surface;
    public int matSLot;
    public Texture idleScreen;
    public Texture renderScreen;


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
        //LiveCam.Clear();
    }

    public static void SetActive(LiveCam liveCam)
    {
        if (activeCam != null)
        {
            activeCam.TurnOff();
        }

        activeCam = liveCam;
        liveCam.TurnOn();
    }

    public void TurnOn()
    {
        cam.enabled = true;
        surface.materials[matSLot].SetTexture("_texture", renderScreen);
    }
    public void TurnOff()
    {
        cam.enabled = false;
        surface.materials[matSLot].SetTexture("_texture", idleScreen);
    }

    public static void Clear()
    {
        if (LiveCam.activeCam == null)
        {
            //Debug.Log("LiveCam cleared, but no LiveCams on");
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
