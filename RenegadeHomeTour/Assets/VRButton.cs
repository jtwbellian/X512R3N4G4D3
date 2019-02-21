using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    public bool on = false;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (on)
        { 
            on = false;
            mat.color = Color.red;
        }
        else
        {
            on = true;
            mat.color = Color.green;
        }

    }
}
