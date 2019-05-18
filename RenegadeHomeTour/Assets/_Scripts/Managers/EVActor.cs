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
        var eventMan = EventManager.GetInstance();

        if (eventMan == null)
        {
            Debug.Log("Event Manager not found, could not unsubscribe");
            return;
        }

        switch (subscribesTo)
        {
            case AppliesTo.ALL:
                eventMan.GlobalEventBegin += OnEventBegin;
                eventMan.PlayerEventBegin += OnEventBegin;
                eventMan.AudioEventBegin += OnEventBegin;
                eventMan.ToolEventBegin += OnEventBegin;
                eventMan.EnvironmentEventBegin += OnEventBegin;
                break;

            case AppliesTo.PLAYER:
                eventMan.PlayerEventBegin += OnEventBegin;
                break;

            case AppliesTo.AUDIO:
                eventMan.AudioEventBegin += OnEventBegin;
                break;

            case AppliesTo.TOOLS:
                eventMan.ToolEventBegin += OnEventBegin;
                break;

            case AppliesTo.ENV:
                eventMan.EnvironmentEventBegin += OnEventBegin;
                break;
        }
    }

    public void Unsubscribe()
    {
        var eventMan = EventManager.GetInstance();

        if (eventMan == null)
        {
            Debug.Log("Event Manager not found, could not unsubscribe");
            return;
        }

        switch (subscribesTo)
        {
            case AppliesTo.ALL:
                eventMan.GlobalEventBegin -= OnEventBegin;
                eventMan.PlayerEventBegin -= OnEventBegin;
                eventMan.AudioEventBegin -= OnEventBegin;
                eventMan.ToolEventBegin -= OnEventBegin;
                eventMan.EnvironmentEventBegin -= OnEventBegin;
                break;

            case AppliesTo.PLAYER:
                eventMan.PlayerEventBegin -= OnEventBegin;
                break;

            case AppliesTo.AUDIO:
                eventMan.AudioEventBegin -= OnEventBegin;
                break;

            case AppliesTo.TOOLS:
                eventMan.ToolEventBegin -= OnEventBegin;
                break;

            case AppliesTo.ENV:
                eventMan.EnvironmentEventBegin -= OnEventBegin;
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


    public abstract void BeginEvent();// {}

    public void CompleteEvent()
    {
        EventManager.CompleteTask(this);
    }

}
