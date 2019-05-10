using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabGame : MonoBehaviour
{

    public VRButton[] switches;
    Director direc;
    public int switchesOn;

    // Start is called before the first frame update
    void Start()
    {
        direc = GameManager.GetInstance().direc;

        if (switches.Length == 0)
        {
            switches = GetComponentsInChildren<VRButton>();
        }
    }

    //Set up the crab game 
    public void PreGame()
    {
        foreach(VRButton vrb in switches)
        {
            vrb.Off();
        }

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
        if (direc.currentLine < 20)
            return;

        foreach (VRButton b in switches)
        {
            if (!b.on)
            {
                return;
            }
        }

        GameManager.GetInstance().direc.Ping(PING.gameWon);
    }
}
