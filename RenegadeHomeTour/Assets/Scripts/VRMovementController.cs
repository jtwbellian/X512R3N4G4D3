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
    private float rechargeRate = 4;
    private Transform body;

    public float speed = 2f;
    public float boost = 1f;

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
        headPos = GetComponentInChildren<Camera>().transform;
        body = GetComponentInChildren<IKPlayerController>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        var stickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        if (Mathf.Abs(stickY) > 0f && boost > 0.5f)
        {

            rb.AddForce(headPos.forward * (speed * stickY), ForceMode.Force);
            boost -= Time.deltaTime * Mathf.Abs(stickY);
        }
        else if (boost < 1f)
        {
            boost += Time.deltaTime * rechargeRate;
        }
    }

}