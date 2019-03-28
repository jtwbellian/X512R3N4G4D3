using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VRButtonSimple : MonoBehaviour
{
    private Collider finger1, finger2;
    private Material mat;
    public UnityEvent trigger;
    public Color color;
    public Color highlightColor;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInChildren<Image>().material;

        foreach (SphereCollider c in GameManager.GetInstance().playerCols)
        {
            if (finger1 == null)
                finger1 = c;
            else if (finger2 == null)
            {
                finger2 = c;
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        mat.SetColor("_tint", highlightColor);
    }

    void OnTriggerExit(Collider other)
    {
        mat.SetColor("_tint", color);
    }

    void OnTriggerStay(Collider col)
    {
        if(col == finger1 || col == finger2)
        if (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Three))
        {
            trigger.Invoke();
        }
    }

}
