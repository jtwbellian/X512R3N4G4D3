using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dallas : EVActor
{
    private enum state { Look, Move}
    [SerializeField]
    private Transform target;
    private ParticleSystem ps;
    private Rigidbody rb;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private Transform home;
    private Transform camView;

    [SerializeField]
    state currentState = state.Look;

    // Start is called before the first frame update
    void Start()
    {

        transform.parent = null;
        ps = GetComponentInChildren<ParticleSystem>();
        target = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        camView = GameManager.GetInstance().hud.hudAnchor.transform;
    }

    void OnEnable()
    {
        //ps.Play();
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
                rb.AddForce((target.position - transform.position) * speed, ForceMode.Force);
                goto case state.Look;
 
            case state.Look:

                var targetRotation = Quaternion.LookRotation(target.position - transform.position);
                var str = Mathf.Min(Time.deltaTime, 1);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
                break;
        }

    }
}
