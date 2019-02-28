using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesDammage : MonoBehaviour
{
    [SerializeField]
    public VRTool tool;
    public bool velocityBased;
    public float power;
    public Rigidbody rb;
    public AudioClip impactSnd;

    public float GetDmg()
    {
        if (velocityBased)
        {
            if (tool == null && rb != null)
            {
                 return power * rb.velocity.magnitude/Time.deltaTime;
            }

            return power * tool.GetVelocity().magnitude / Time.deltaTime;
        }

        return power;
    }

}
