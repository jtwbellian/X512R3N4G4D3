using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void EventBeginHandler(EventInfo info);

public class EventManager : MonoBehaviour
{
    public static EventManager _instance = null;
    public static bool sandboxMode = false;

    [SerializeField]
    public List<EventInfo> timeline;
    [SerializeField]
    public int currentEvent = 0;

    #region singleton

    public static EventManager GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        if (_instance == this)
        {
            return;
        }

        if (_instance != null)
        { 
            Destroy(_instance);
            _instance = this;
            EventManager.sandboxMode = false;
        }
        else
        {
            _instance = this;
            EventManager.sandboxMode = false;
        }

    }

    #endregion
    
    public event EventBeginHandler PlayerEventBegin;
    public event EventBeginHandler AudioEventBegin;
    public event EventBeginHandler ToolEventBegin;
    public event EventBeginHandler EnvironmentEventBegin;
    public event EventBeginHandler GlobalEventBegin;

    #region eventDesignerTools
    // These functions make it easier to change the timeline in the editor/at runtime
    
    public void SandboxModeOn()
    {
        EventManager.sandboxMode = true;
        EventInfo disableWalls = new EventInfo();
        disableWalls.myName = "walls";
        disableWalls.type = EV.ItemDropped;
        disableWalls.target = AppliesTo.ENV;
        EnvironmentEventBegin.Invoke(disableWalls);
    }

    [ContextMenu("InsertAtCurrentEvent")]
    private void InsertEvent()
    {
        timeline.Insert(currentEvent, new EventInfo());
    }

    [ContextMenu("RemoveAtCurrentEvent")]
    private void RemoveEvent()
    {
        timeline.RemoveAt(currentEvent);
    }

    [ContextMenu("PreviousEvent")]
    private void PreviousEvent()
    {
        --currentEvent;
        StartEvent();
    }
    [ContextMenu("SkipEvent")]
    private void SkipEvent()
    {
        ++currentEvent;
        StartEvent();
    }

    #endregion

    // This function is called when an actor completes the current task. 
    // This works for a linear timeline but will need to consider multiple tasks in the future
    public static void CompleteTask(EVActor actor)
    {
        if (actor.myEvent == EventManager.GetCurrentEvent())
        {
            EventManager.GetInstance().currentEvent++;
            EventManager.GetInstance().StartEvent();
            //Debug.Log("Event [" + actor.myEvent + "] completed by " + actor.myName);
        }
    }

    public static EventInfo GetCurrentEvent()
    {
        return EventManager.GetInstance().timeline[EventManager.GetInstance().currentEvent];
    }

    // Invokes the starting event of all subcribers within the current events group
    // ie, every Actor subscribes to Global Events, only Actors with audio/dialogue 
    // subscribe to AudioEvents, Only inanimate objects subsribe to Tool/Environment
    [ContextMenu("StartEvent")]
    public void StartEvent()
    {
        if (currentEvent >= timeline.Count)
        {
            return;
        }

        switch (timeline[currentEvent].target)
        {
            case AppliesTo.ALL:
                GlobalEventBegin.Invoke(timeline[currentEvent]);
                PlayerEventBegin.Invoke(timeline[currentEvent]);
                AudioEventBegin.Invoke(timeline[currentEvent]);
                ToolEventBegin.Invoke(timeline[currentEvent]);
                EnvironmentEventBegin.Invoke(timeline[currentEvent]);
                break;

            case AppliesTo.PLAYER:
                PlayerEventBegin.Invoke(timeline[currentEvent]);
                break;

            case AppliesTo.AUDIO:
                AudioEventBegin.Invoke(timeline[currentEvent]);
                break;

            case AppliesTo.TOOLS:
                ToolEventBegin.Invoke(timeline[currentEvent]);
                break;

            case AppliesTo.ENV:
                EnvironmentEventBegin.Invoke(timeline[currentEvent]);
                break;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
