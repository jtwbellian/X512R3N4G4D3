using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeEditMenu: MonoBehaviour {

    private GameObject menuOffCanvas;
    private GameObject menuOnCanvas;
    private bool isMenuUp = false;

    private float fadeTime = 1f;
    private float fadeStart;
    private Vector3 minScale = new Vector3(0.002f, 0f, 0.002f);
    private Vector3 fullScale = new Vector3(0.002f, 0.002f, 0.002f);


    // Use this for initialization
    void Start() {

        menuOffCanvas = GameObject.FindGameObjectsWithTag("MenuOff")[0];
        menuOnCanvas = GameObject.FindGameObjectsWithTag("MenuOn")[0];


        fadeStart = Time.time;
        menuOffCanvas.transform.localScale = fullScale;
        menuOnCanvas.transform.localScale = minScale;

        HideMenu();

    }

    // Update is called once per frame
    void Update() {

        // If the menu is up, shrink the menu off canvas and scale the menu
        if (isMenuUp && menuOnCanvas.transform.localScale != fullScale)
        {
            float t = (Time.time - fadeStart) / fadeTime;

            menuOffCanvas.transform.localScale = Vector3.Lerp(fullScale, minScale, t);
            menuOnCanvas.transform.localScale = Vector3.Lerp(minScale, fullScale, t);
        }

        // Alternatively, shrink the menu canvas and scale the menu off canvas
        if (!isMenuUp && menuOffCanvas.transform.localScale != fullScale)
        {
            float t = (Time.time - fadeStart) / fadeTime;

            menuOffCanvas.transform.localScale = Vector3.Lerp(minScale, fullScale, t);
            menuOnCanvas.transform.localScale = Vector3.Lerp(fullScale, minScale, t);
        }
    }

    void OnClick()
    {
        if (!isMenuUp)
            ShowMenu();
    }

    // Hides the interactive menu and shows the Binari Logo
    void HideMenu()
    {
        isMenuUp = false;
        fadeStart = Time.time;

    }

    // Hides the logo reveals the interactive menu
    void ShowMenu()
    {
        isMenuUp = true;
        fadeStart = Time.time;
    }

}
