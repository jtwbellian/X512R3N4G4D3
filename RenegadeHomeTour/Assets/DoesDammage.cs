using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesDammage : MonoBehaviour
{
    private const float MAX_DAMMAGE = 500f;
    private bool active = true;
    [SerializeField]
    public VRTool tool;
    public bool velocityBased;
    public float power;
    public Rigidbody rb;
    public AudioClip impactSnd;

    public void Enable()
    {
        active = true;
    }

    public void Disable()
    {
        active = false;
    }

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
            Destroy(this);
        }
    }

    public float GetDmg()
    {
        if (active)
        {
            if (velocityBased)
            {
                if (tool == null && rb != null)
                {
                    return Mathf.Max(power * rb.velocity.magnitude / Time.deltaTime, MAX_DAMMAGE);
                }

                return Mathf.Max(power * tool.GetVelocity().magnitude / Time.deltaTime * 4, MAX_DAMMAGE);
            }

            return Mathf.Max(power, MAX_DAMMAGE);
        }

        return 0f;
    }


}
