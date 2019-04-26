using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_grappleHook : MonoBehaviour
{
    private Rigidbody rb;
    public vrt_grapple gun;
    private bool grabbed;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        grabbed = false;
        rb = GetComponent<Rigidbody>();    
    }

    void OnColliderEnter(Collider col)
    {
        if (!grabbed)
        {
            gun.Retract();
            grabbed = true;
            anim.SetBool("Extend", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
