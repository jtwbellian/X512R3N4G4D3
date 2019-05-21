using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dallas : EVActor
{
    private float LastDeparture;
    private float timeOut = 500;
    private const float MIN_DIST = 0.1f;
    private const float MAX_DIST = 0.25f;
    private enum state { Look, Move, Follow}
    [SerializeField]
    private Animator anim;
    private Transform target;
    private ParticleSystem ps;
    private Rigidbody rb;
    private Quaternion targetRotation;
    private float str;
    private GameManager gm;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private Transform home;
    private Transform camView;
    private Vector3 lastPos;
    private bool waitForMe = false;
    private bool isHome = false;
    public GameObject myItem; 

    [SerializeField]
    state currentState = state.Look;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        subscribesTo = AppliesTo.ENV;
        Subscribe();
        transform.parent = null;
        ps = GetComponentInChildren<ParticleSystem>();
        target = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        camView = GameManager.GetInstance().hud.hudAnchor.transform;
        anim = GetComponentInChildren<Animator>();
        gm = GameManager.GetInstance();
    }

    void OnEnable()
    {
        //ps.Play();
    }

    public void DropItem()
    {
        if (myItem == null)
            return;

        anim.Play("DallasBones|OpeningState");
        myItem.SetActive(true);
        myItem.transform.SetParent(null);
        myItem = null;
    }

    public override void BeginEvent()
    {
        switch (myEvent.type)
        {
            case EV.GoHome:
                waitForMe = true;
                GoHome();
                break;

            case EV.targetHit:
                waitForMe = true;
                break;

            case EV.ItemDropped:
                myItem.SetActive(true);
                myItem.transform.SetParent(null);
                CompleteEvent();
                break;

            default:
                break;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        var dest = col.GetComponent<Destination>();

        if (dest == null)
            return;

        if (target == dest.transform)
        {
            if (dest.nextDestination == null)
            {
                Stop();
            }
            else
                target = dest.nextDestination;
        }
    }

    [ContextMenu("FollowPlayer")]
    public void FollowPlayer()
    {
        target = camView;
        currentState = state.Move;
        LastDeparture = Time.time;
    }

    [ContextMenu("Seek")]
    public void Watch()
    {
        currentState = state.Look;
    }

    [ContextMenu("GoHome")]
    public void GoHome()
    {
        LastDeparture = Time.time;
        target = home;
        currentState = state.Move;
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        currentState = state.Look;
        rb.velocity = Vector3.zero;
        target = camView;
    }
   

    public void SetHome(Transform obj)
    {
        home = obj;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
            
        switch(currentState)
        {
            case state.Move:

                if (Time.time > LastDeparture + timeOut)
                {
                    transform.position = target.transform.position;
                    rb.velocity = Vector3.zero;
                }

                if (Vector3.Distance(target.position, transform.position) < MIN_DIST)
                {
                    if (!isHome && target == home)
                    {
                        if (waitForMe && myEvent.type == EV.GoHome)
                            CompleteEvent();
                        isHome = true;
                    }

                    lastPos = target.position;

                    var dest = target.GetComponent<Destination>();

                    if (dest == null)
                    {
                        target = camView;
                    }
                    else
                    {
                        if (dest.nextDestination == null)
                        {
                            Stop();
                            home = target;

                            if (waitForMe && myEvent.type == EV.targetHit)
                                CompleteEvent();

                            currentState = state.Look;
                            anim.Play("DallasBones|OpeningState");
                        }
                        else
                            target = dest.nextDestination;
                    }
                }

                targetRotation = Quaternion.LookRotation(target.position - transform.position);
                str = Mathf.Min(Time.deltaTime, 3);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

                rb.AddForce((target.position - transform.position) * speed, ForceMode.Force);
                break;

            case state.Follow:
                targetRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
                str = Mathf.Min(Time.deltaTime, 1);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

                rb.AddForce(((gm.hud.hudAnchor.position + gm.hud.hudAnchor.right * 2f) - transform.position) * speed, ForceMode.Force);
                break;

            case state.Look:

                targetRotation = Quaternion.LookRotation(target.position - transform.position);
                str = Mathf.Min(Time.deltaTime, 1);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

                if (Vector3.Distance(lastPos, transform.position) > MAX_DIST)
                {
                    rb.AddForce((lastPos - transform.position) * speed/2, ForceMode.Force);
                }

                break;
        }

    }
}
