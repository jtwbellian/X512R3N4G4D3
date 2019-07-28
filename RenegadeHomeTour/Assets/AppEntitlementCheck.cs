using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Oculus.Platform;
using UnityEngine.SceneManagement;


public class AppEntitlementCheck : MonoBehaviour {

    public GameObject ErrorMsg;
    public bool quitting;
    public bool failed = false;

    public bool on = false;

    void Start () {
        if (!on)
            return;

        Oculus.Platform.Core.Initialize();
    }

    void Update (){
        if (!on)
            return;

        CheckApplicationEntitlement();
        Request.RunCallbacks();
    }

    public void CheckApplicationEntitlement() {
        Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(callbackMethod);
    }

    void callbackMethod(Message msg)
    {
        if (!msg.IsError)
        {
            Debug.Log ("Good");
        }
        else
        {
            Debug.Log ("Bad");
            failed = true;
            if (!quitting) {
                StartCoroutine (quitApp ());
                quitting = true;
            }
        }
    }

    IEnumerator quitApp ()
    {
        //Instantiate (ErrorMsg);
        yield return new WaitForSeconds (30);
        UnityEngine.Application.Quit ();
        Debug.Log ("GoodBye");
    }


}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;

public class AppEntitlementCheck : MonoBehaviour
{
    public bool failed = false;
    public string ID = "2482852541734304";

    void Awake()
    {
        try
        {
            Core.AsyncInitialize();
            Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
        }
        catch (UnityException e)
        {
            print("Oculus Platform failed to initialize due to exception." + e.ToString());
            Debug.LogException(e);
            failed = true;
            // Immediately quit the application
            //UnityEngine.Application.Quit();
        }
    }

    void EntitlementCallback(Message msg)
    {
        if (msg.IsError)
        {
//            print(msg.GetError());
            print("Oculus failed to init due to exception ");
            failed = true;
            //UnityEngine.Application.Quit();
        }
        else
        {
            Debug.Log("Oculus entitlement passed.");
        }
    }
}
*/