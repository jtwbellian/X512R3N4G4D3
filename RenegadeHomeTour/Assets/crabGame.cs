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

    // Update is called once per frame
    void Update()
    {
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
