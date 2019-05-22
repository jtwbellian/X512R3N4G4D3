using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabGame : EVActor
{
    public VRButton[] switches;
    Director direc;
    public int switchesOn;

    // Start is called before the first frame update
    void Start()
    {
        subscribesTo = AppliesTo.ENV;
        myName = "Game";
    }

    public override void BeginEvent()
    {
        if (myEvent.type == EV.GameStart)
        {
            SwitchesOff();
        }
    }

    //Set up the crab game 
    public void PreGame()
    {
        /*foreach(VRButton vrb in switches)
        {
            vrb.Off();
        }*/

        Hud hud = GameManager.GetInstance().hud;
        hud.ShowKills();
    }

    public void SwitchesOn()
    {
        foreach (VRButton vrb in switches)
        {
            vrb.On();
        }
    }

    public void SwitchesOff()
    {
        foreach (VRButton vrb in switches)
        {
            vrb.Off();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (switchesOn == 0 && myEvent.type == EV.GameEnd)
        {
            CompleteEvent();
        }
        /*
         if (direc.currentLine < 20)
            return;
            */
    }
}
