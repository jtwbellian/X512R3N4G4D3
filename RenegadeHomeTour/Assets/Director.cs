using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PING
{
    NONE = -1,
    calibrated,
    ItemGrabbed,
    gunFired,
    ItemDropped,
    targetHit,
    analogFwd,
    gameWon,
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
    public int currentLine = 0;
    public Line[] script;
    public bool autoStart = false;
    public bool trainingMode = true;
    public bool wasInvoked = false;
    

    // Start is called before the first frame update
    void Start()
    {
        sm = SoundManager.GetInstance();

        if (autoStart)
        {
            Invoke("Action", 1f);
        }
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }


    public void EndTrainingMode()
    {
        trainingMode = false;
    }
    public void Ping(PING ping)
    {
        if (currentLine >= script.Length)
        {
            Debug.Log("End of Script Reached");
            return;
        }

        //Debug.Log("Ping recieved with " + ping.ToString());

        if ( ! wasInvoked && ping.Equals(script[currentLine].waitFor))
        {
            script[currentLine].onEnd.Invoke();
            Invoke("Action", script[currentLine].margin);
            currentLine++;
            wasInvoked = true;
        }
        else if (trainingMode && ping.Equals(PING.ItemDropped))
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
        wasInvoked = false;

        if (currentLine >= script.Length)
        {
            Debug.Log("End of Script Reached");
            return;
        }

        Debug.Log("Action Called for event ");
        script[currentLine].onStart.Invoke();

        sm.dialogue.clip = script[currentLine].clip;
        sm.dialogue.Play();

        if (script[currentLine].waitFor == PING.NONE)
        {
            script[currentLine].onEnd.Invoke();
            Invoke("Action", script[currentLine].margin);
            currentLine++;
        }
    }
}
