using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooty : EVActor
{
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
                fx = FXManager.GetInstance();
                fx.Burst(FXManager.FX.Shock, transform.position,3);
                CompleteEvent();
                audio.Play();
                break;

            case EV.ItemDropped:
                target.SetActive(false);
                fx.Burst(FXManager.FX.Shock, transform.position, 3);
                CompleteEvent();
                break;
        }

    }

    void OnTriggerEnter(Collider other)
    {

        if (myEvent.type == EV.gunFired && target.activeSelf && other.GetComponent<DoesDammage>())
        {
            target.SetActive(false);
            fx.Burst(FXManager.FX.Shock, transform.position, 3);
            CompleteEvent();
        }

    }

}
