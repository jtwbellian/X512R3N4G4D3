using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooty : EVActor
{

    private float distToHead = 2f;
    [SerializeField]
    GameObject target;
    FXManager fx;
    AudioSource audio;
    

    // Start is called before the first frame update
    void Start()
    {
        fx = FXManager.GetInstance();
        audio = GetComponent<AudioSource>();
        subscribesTo = AppliesTo.ENV;
    }

    public override void BeginEvent()
    {
        switch (myEvent.type)
        {
            case EV.ItemGrabbed:
                target.SetActive(true);
                audio.Play();
                //transform.position = Camera.main.transform.position + Camera.main.transform.forward * distToHead;
                fx = FXManager.GetInstance();
                fx.Burst(FXManager.FX.Shock, transform.position, 15);
                CompleteEvent();
                audio.Play();
                break;

            case EV.ItemDropped:
                target.SetActive(false);
                fx.Burst(FXManager.FX.Shock, transform.position, 15);
                CompleteEvent();
                break;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (myEvent == null)
            return;

        if (myEvent.type == EV.gunFired)
        {
            DoesDammage dd = other.transform.GetComponent<DoesDammage>();
            if (dd == null)
                return;

            target.SetActive(false);
            fx.Burst(FXManager.FX.Shock, transform.position, 15);
            CompleteEvent();
        }
    }

}
