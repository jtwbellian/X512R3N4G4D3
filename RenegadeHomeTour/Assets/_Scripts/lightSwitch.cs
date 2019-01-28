using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSwitch : MonoBehaviour
{


    private bool lightOn = true;
    private float coolDown = 0;

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightHand" && coolDown <= 0)
        {
            coolDown = 200;

            if (lightOn) // Turn light off
            {
                lightOn = false;
                
            }
            else // Turn light on 
            {
                lightOn = true;
            }
        }
    }

    void Update()
    {
        if (coolDown > 0)
            coolDown -= 1;
    }


}