using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PING
{
    NONE = -1,
    calibrated,
    GunGrabbed,
    gunFired,
    gunDropped,
    targetHit,
    gameWon
}

[System.Serializable]
public class Line
{
    public AudioClip clip;
    public float margin;
    public PING waitFor;
    public Icon icon;
    public UnityEvent onStart;
    public UnityEvent onEnd;
}

public class Director : MonoBehaviour
{
    private string lastAction;
    private SoundManager sm;
    [SerializeField]
    private int currentLine = 0;
    public Line[] script;
    public bool autoStart = false;
    public bool trainingMode = true;
    

    // Start is called before the first frame update
    void Start()
    {
        sm = SoundManager.GetInstance();

        if (autoStart)
        {
            Invoke("Action", 1f);
        }
    }

    public void Ping(PING ping)
    {
        Debug.Log("Ping recieved with " + ping.ToString());

        if (ping.Equals(script[currentLine].waitFor))
        {
            script[currentLine].onEnd.Invoke();
            currentLine++;
            Invoke("Action", script[currentLine].margin);
        }else if (trainingMode && ping.Equals(PING.gunDropped))
        {
            // Have Dallas remind player to pick up gun from holster
        }

    }

    public Icon GetIcon()
    {
        return script[currentLine].icon;
    }

    public void Action()
    {
        Debug.Log("Action Called for event ");
        script[currentLine].onStart.Invoke();

        sm.dialogue.PlayOneShot(script[currentLine].clip);

        if (script[currentLine].waitFor == PING.NONE)
        {
            script[currentLine].onEnd.Invoke();
            currentLine++;
            Invoke("Action", script[currentLine].margin);
        }
    }
}
