using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesDammage : MonoBehaviour
{
    private const float MAX_DAMMAGE = 500f; 
    [SerializeField]
    public VRTool tool;
    public bool velocityBased;
    public float power;
    public Rigidbody rb;
    public AudioClip impactSnd;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("target"))
        {
            GameManager.GetInstance().direc.Ping(PING.targetHit);
            var audio = col.transform.GetComponent<AudioSource>();
            MeshRenderer renderer = col.transform.GetComponent<MeshRenderer>();

            if (renderer !=  null)
                renderer.material.SetFloat("Boolean_4AD6AAEC", 1f);

            if (audio != null)
            {
                audio.Play();
            }
                //col.gameObject.SetActive(false);
        }
    }

    public float GetDmg()
    {
        if (velocityBased)
        {
            if (tool == null && rb != null)
            {
                 return Mathf.Max(power * rb.velocity.magnitude / Time.deltaTime, MAX_DAMMAGE);
            }

            return Mathf.Max(power * tool.GetVelocity().magnitude / Time.deltaTime, MAX_DAMMAGE);
        }

        return Mathf.Max(power, MAX_DAMMAGE);
    }

}
