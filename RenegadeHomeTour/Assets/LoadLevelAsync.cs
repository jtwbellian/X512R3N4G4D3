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
    

    // Start is called before the first frame update
    void Start()
    {
        hud.message = "Hold Any Key to Start";
        hud.ShowSubtitles();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Any) && !ready)
        {
            if (fill > 0.9)
            {
                screenFade.currentAlpha = 1f;
                screenFade.SetMaterialAlpha();
                SceneManager.LoadSceneAsync(level);
                ready = true;
            }
            else
            {
                fill += 1 * Time.deltaTime;
                screenFade.currentAlpha = fill;
                screenFade.SetMaterialAlpha();
            }
        }
        else if (!ready)
        {
            fill -= 1 * Time.deltaTime;
            screenFade.currentAlpha = fill;
            screenFade.SetMaterialAlpha();
        }
    }
}