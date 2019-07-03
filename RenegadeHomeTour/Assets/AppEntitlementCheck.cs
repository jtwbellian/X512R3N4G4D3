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
            print(msg.GetError());
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