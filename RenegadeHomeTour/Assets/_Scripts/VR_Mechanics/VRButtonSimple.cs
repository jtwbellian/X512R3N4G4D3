using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VRButtonSimple : MonoBehaviour
{
    private Collider finger1, finger2;
    private SpriteRenderer mat;

    public UnityEvent trigger;
    public Color color;
    public Color highlightColor;
    bool canPush = false;

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
        Invoke("AllowPush", 1.5f);
    }

    public void AllowPush()
    {
        canPush = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canPush)
            return;

        if (other.CompareTag("RightHand"))
        {
            OVRHapticsManager.GetInstance().BuzzRight(VibrationForce.Medium, 0.05f);
            canPush = false;
        }
        else if (other.CompareTag("LeftHand"))
        {
            canPush = false;
            OVRHapticsManager.GetInstance().BuzzLeft(VibrationForce.Medium, 0.05f);
        }

        if (mat)
            mat.color = highlightColor;
    }

    void OnTriggerExit(Collider other)
    {
        if (!canPush && (other.CompareTag("RightHand") || other.CompareTag("LeftHand")))
        {
            if (mat)
                mat.color = color;

            canPush = true;
            trigger.Invoke();
        }
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
