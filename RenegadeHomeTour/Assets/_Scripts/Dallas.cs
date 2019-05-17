using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dallas : EVActor
{
    private const float MIN_DIST = 0.5f;
    private enum state { Look, Move, Follow}
    [SerializeField]
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
    private bool isHome = false;

    [SerializeField]
    state currentState = state.Look;

    // Start is called before the first frame update
    void Start()
    {
        subscribesTo = AppliesTo.ENV;
        transform.parent = null;
        ps = GetComponentInChildren<ParticleSystem>();
        target = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        camView = GameManager.GetInstance().hud.hudAnchor.transform;
        gm = GameManager.GetInstance();
    }

    void OnEnable()
    {
        //ps.Play();
    }

    public override void BeginEvent()
    {
        switch (myEvent.type)
        {
            case EV.GoHome:
                GoHome();
                EventManager.CompleteTask(this);
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

    public void FollowPlayer()
    {
        target = camView;
        currentState = state.Move;
    }

    public void GoHome()
    {
        target = home;
        currentState = state.Move;
    }

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
            return;

        switch(currentState)
        {
            case state.Move:
                if (Vector3.Distance(target.position, transform.position) < MIN_DIST)
                {
                    if (!isHome && target == home)
                    {
                        isHome = true;
                    }
                    return;
                }

                rb.AddForce((target.position - transform.position) * speed, ForceMode.Force);
                goto case state.Look;

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
                break;
        }

    }
}
