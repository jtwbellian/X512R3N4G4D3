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
            if (_instance != null)
                Destroy(this);
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
