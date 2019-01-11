using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPointer : MonoBehaviour
{

    private Transform prevUIHit;

    private Image buttonImage;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

    }

    /*void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "UIButton")
            OnTriggerEnter(col);
    }*/


    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "StartButton")
        {
            col.transform.SendMessage("OnClick");
        }

        if (col.transform.tag == "UIButton")
        {
            buttonImage = col.transform.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = new Color(0.6f, 0.6f, 0.6f, 1);
            }

            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || OVRInput.Get(OVRInput.Button.One))
            {
                col.transform.SendMessage("OnClick");
            }
        }
    }

}