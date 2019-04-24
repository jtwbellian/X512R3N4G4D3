using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSwitch : MonoBehaviour
{


    public Light myLight;

    // Use this for initialization
    void Start()
    {
        if (myLight == null)
            myLight = GetComponentInChildren<Light>();
    }

    public void ChangeRed()
    {
        myLight.color = Color.red;
    }
    /*
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
    }*/




}