using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRMovementController : MonoBehaviour
{

    private const float MAX_SPEED = 20f;
    private float boostbar_width = 0.0f;
    private Rigidbody rb;
    private Transform headPos;
    private float rechargeRate = 0.5f;
    private Transform body;
    private bool ReadyToSnapTurn = false;


    public float speed = 2f;
    public float boost = 1f;
    public float rotationRatchet = 45f;

    public Text velocityLabel;
    public Transform boostBar;


    // Start is called before the first frame update
    void Start()
    {
        if (boostBar == null)
            Debug.Log("ERROR: Boost bar is null, dummy!");
        else
            boostbar_width = boostBar.localScale.x;

        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;
        headPos = GetComponentInChildren<Camera>().transform;
        body = GetComponentInChildren<IKPlayerController>().transform;

    }

    // Update is called once per frame
    void Update()
    {

        // Use Boost Jets
        var stickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        if (Mathf.Abs(stickY) > 0f && boost > 0f)
        {

            rb.AddForce(headPos.forward * (speed * stickY * boost), ForceMode.Force);
            boost -= Time.deltaTime * Mathf.Abs(stickY);
        }
        else if (boost < 1f)
        {
            boost += Time.deltaTime * rechargeRate;
        }

        boostBar.localScale = new Vector3(boostbar_width * boost, boostBar.localScale.y, boostBar.localScale.z);
        
        // Turn view 
        Vector3 euler = transform.rotation.eulerAngles;

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            if (ReadyToSnapTurn)
            {
                euler.y -= rotationRatchet;
                ReadyToSnapTurn = false;
            }
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            if (ReadyToSnapTurn)
            {
                euler.y += rotationRatchet;
                ReadyToSnapTurn = false;
            }
        }
        else
        {
            ReadyToSnapTurn = true;
        }

        transform.rotation = Quaternion.Euler(euler);
    }

}