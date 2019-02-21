using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.Log("Animator not found!");
        }
    }

    public void Flap()
    {
        Debug.Log("flap triggered");

        anim.speed = 1f;
        //anim.SetBool("Flap", true);
        anim.Play("swing",0);
    }
}
