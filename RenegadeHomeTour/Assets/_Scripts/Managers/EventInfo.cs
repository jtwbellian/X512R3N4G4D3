using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AppliesTo { ALL, PLAYER, AUDIO, TOOLS, ENV}

public enum EV
{
    NONE = -1,
    Calibrated,
    ItemGrabbed,
    gunFired,
    ItemDropped,
    targetHit,
    analogFwd,
    gameWon,
    weaponHolstered,
    audioStart,
    audioWait,
    EntersTrigger,
    ExitsTrigger,
    GoHome
}

[System.Serializable, CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EventInfo")]
public class EventInfo : ScriptableObject
{
    public string myName = "NEW EVENT";
    public EV type = EV.NONE;
    public AppliesTo target;
    public string tag = "NONE";

    public override string ToString()
    {
        return myName + " wait for " + type.ToString() + " (tag: " + tag + ")";
    }
}


