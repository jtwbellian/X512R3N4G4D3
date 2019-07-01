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
            Core.AsyncInitialize(ID);
            Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
        }
        catch (UnityException e)
        {
            Debug.LogError("Oculus Platform failed to initialize due to exception.");
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
            print("Failure");
            Debug.LogError("Oculus entitlement check FAILED.");
            failed = true;
            //UnityEngine.Application.Quit();
        }
        else
        {
            print("Pass");
            Debug.Log("Oculus entitlement passed.");
        }
    }
}