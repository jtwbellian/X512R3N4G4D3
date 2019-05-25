using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VRButtonSimple : MonoBehaviour
{
    private Collider lastCol;
    private Collider finger1, finger2;
    private SpriteRenderer mat;

    public UnityEvent trigger;
    public Color color;
    public Color highlightColor;
    bool canPush = false;

    bool touch = false;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>();
        /*
        foreach (Vs c in GameManager.GetInstance().playerCols)
        {
            if (finger1 == null)
                finger1 = c;
            else if (finger2 == null)
            {
                finger2 = c;
                break;
            }
        }*/
    }

    void OnEnable()
    {
        Invoke("AllowPush", .5f);

        if (mat)
            mat.color = highlightColor;
    }

    void OnDisable()
    {
        canPush = false;
        touch = false;
    }

    public void AllowPush()
    {
        canPush = true;

        if (mat)
            mat.color = color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canPush || touch)
            return;

        if (other.gameObject.name.Contains("index_03") &&other.gameObject.CompareTag("RightHand"))
        {
            OVRHapticsManager.GetInstance().BuzzRight(VibrationForce.Medium, 0.05f);
        }
        else if (other.gameObject.name.Contains("index_03") && other.gameObject.CompareTag("LeftHand"))
        {
            OVRHapticsManager.GetInstance().BuzzLeft(VibrationForce.Medium, 0.05f);
        }

        if (mat)
            mat.color = highlightColor;

        touch = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (touch)//&& other.gameObject.name.Contains("index_03"))
        {
            touch = false;
            trigger.Invoke();
        }

        if (mat)
            mat.color = color;
    }

    /* void OnTriggerStay(Collider col)
     {
         if(col.CompareTag("RightHand") || col.CompareTag("LeftHand"))
         if (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Three))
         {
             trigger.Invoke();
         }
     }*/

}
