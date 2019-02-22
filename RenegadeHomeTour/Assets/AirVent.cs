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

    public void Reset()
    {
        anim.SetBool("Flap", false);
    }

    public void Flap()
    {

        if (anim == null)
            anim = GetComponent<Animator>();

        anim.speed = 1f;
        anim.SetBool("Flap", true);
        Invoke("Reset", 2f);
    }
}
