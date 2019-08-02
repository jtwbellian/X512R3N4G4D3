using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureHandler : MonoBehaviour
{
    public Transform gaze;
    public OVRGrabber handR, handL;

    private IKPlayerController iKPlayerController;
    private RaycastHit hit;
    private int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        iKPlayerController = GetComponent<IKPlayerController>();
        layerMask = LayerMask.GetMask("Ignore Raycast");
    }

    // Update is called once per frame
    void Update()
    {
        if (iKPlayerController.OpenHand(false))
        {

            if (Physics.Raycast(gaze.transform.position, gaze.transform.forward, out hit, 4f, layerMask))
            {
                Dallas hitDallas = hit.transform.gameObject.GetComponent<Dallas>();
                
                if (!hitDallas)
                    return;

                if (hitDallas.rb.velocity.magnitude > 5f)
                {
                    return;
                }

                
                if ((handR.GetVelocity().magnitude > 0.01f && handR.transform.position.y > (iKPlayerController.head.transform.position.y - iKPlayerController.height * 0.6f) && iKPlayerController.OpenHand(false)) || 
                (handL.GetVelocity().magnitude > 0.1f && handL.transform.position.y > (iKPlayerController.head.transform.position.y - iKPlayerController.height * 0.6f)) && iKPlayerController.OpenHand(true))
                {
                    Debug.Log("Wave recognized");
                    hitDallas.SayHello();
                }
            }

            //if (handR.GetVelocity().magnitude > 0.03f )
            //Debug.Log("Rhand- Velocity: " + handR.GetVelocity().ToString() + ", MAG: " + handR.GetVelocity().magnitude + ", Ang: " + handR.GetAngVelocity());
        }
    }
}
