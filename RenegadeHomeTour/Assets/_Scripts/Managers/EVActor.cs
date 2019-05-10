using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void RecieveMessage(EVActor sender);

public abstract class EVActor : MonoBehaviour
{
    [Header("Actor Settings")]
    public string myName = "NoName";
    public EventInfo myEvent;
    protected AppliesTo subscribesTo;

    [ContextMenu("Subscribe")]
    public void Subscribe()
    {
        Debug.Log( myName + " Subscribed to " + subscribesTo);
        switch (subscribesTo)
        {
            case AppliesTo.ALL:
                EventManager.GetInstance().GlobalEventBegin += OnEventBegin;
                EventManager.GetInstance().PlayerEventBegin += OnEventBegin;
                EventManager.GetInstance().AudioEventBegin += OnEventBegin;
                EventManager.GetInstance().ToolEventBegin += OnEventBegin;
                EventManager.GetInstance().EnvironmentEventBegin += OnEventBegin;
                break;

            case AppliesTo.PLAYER:
                EventManager.GetInstance().PlayerEventBegin += OnEventBegin;
                break;

            case AppliesTo.AUDIO:
                EventManager.GetInstance().AudioEventBegin += OnEventBegin;
                break;

            case AppliesTo.TOOLS:
                EventManager.GetInstance().ToolEventBegin += OnEventBegin;
                break;

            case AppliesTo.ENV:
                EventManager.GetInstance().EnvironmentEventBegin += OnEventBegin;
                break;
        }
    }

    public void Unsubscribe()
    {
        switch (subscribesTo)
        {
            case AppliesTo.ALL:
                EventManager.GetInstance().GlobalEventBegin -= OnEventBegin;
                EventManager.GetInstance().PlayerEventBegin -= OnEventBegin;
                EventManager.GetInstance().AudioEventBegin -= OnEventBegin;
                EventManager.GetInstance().ToolEventBegin -= OnEventBegin;
                EventManager.GetInstance().EnvironmentEventBegin -= OnEventBegin;
                break;

            case AppliesTo.PLAYER:
                EventManager.GetInstance().PlayerEventBegin -= OnEventBegin;
                break;

            case AppliesTo.AUDIO:
                EventManager.GetInstance().AudioEventBegin -= OnEventBegin;
                break;

            case AppliesTo.TOOLS:
                EventManager.GetInstance().ToolEventBegin -= OnEventBegin;
                break;

            case AppliesTo.ENV:
                EventManager.GetInstance().EnvironmentEventBegin -= OnEventBegin;
                break;
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Subscribe();
    }


    void OnDisable()
    {
        Unsubscribe();
    }

    public void OnEventBegin(EventInfo info)
    {
        if (info.tag != "" && info.tag != "NONE" && info.tag != null)
        {
            if (CompareTag(info.tag))
            {
                myEvent = info;
                BeginEvent();
                return;
            }
        }

        if (info.myName == myName)
        {
            myEvent = info;
            BeginEvent();
        }
    }


    public virtual void BeginEvent() {}

    public void CompleteEvent()
    {
        EventManager.CompleteTask(this);
    }

}
