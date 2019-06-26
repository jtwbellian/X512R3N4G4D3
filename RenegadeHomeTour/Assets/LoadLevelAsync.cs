using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{
    public Hud hud;
    public string level;
    public OVRScreenFade screenFade; 
    private bool ready = false;
    private float fill = 0f;
    private bool startLoading = false;
    private AsyncOperation asyncLoad;
    public AppEntitlementCheck entitlementChecker;

    // Start is called before the first frame update
    void Start()
    {
        hud.message = "Hold Any Button to Start";
        hud.ShowSubtitles();
    }

    // Update is called once per frame
    void Update()
    {
        if (entitlementChecker.failed)
        {
            hud.message = "Entitlement Check Failed. Please confirm Oculus Account is signed in.";
            hud.ShowSubtitles();
            hud.Refresh();
            return;
        }

        if (OVRInput.Get(OVRInput.Button.Any))
        {
            if (fill > 1 && !ready)
            {
                screenFade.currentAlpha = 1f;
                screenFade.SetMaterialAlpha();
                SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
                ready = true;
            }
            else
            {
                fill += 1 * Time.deltaTime;
                screenFade.currentAlpha = fill;
                screenFade.SetMaterialAlpha();
            }
        }
        else if (fill > 0 && !ready)
        {
            fill -= 1 * Time.deltaTime;
            screenFade.currentAlpha = fill;
            screenFade.SetMaterialAlpha();
        }
    }
}