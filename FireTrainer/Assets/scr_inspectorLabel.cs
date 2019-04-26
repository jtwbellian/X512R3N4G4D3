using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scr_inspectorLabel : MonoBehaviour {
    public Transform target;
    public string message;
    [HideInInspector]
    public bool isOn;

    [SerializeField]
    private Text textfield;
    [SerializeField]  
    private Text textfield_s;

    private CanvasGroup canvas; 

    // Use this for initialization
    void Start () {
        textfield.text = message;
        textfield_s.text = message;
        canvas = GetComponentInChildren<CanvasGroup>();
        isOn = false;
        canvas.alpha = 0f;
	}

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeTo(1f, 1f));
        isOn = true;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeTo(0f, 1f));
        isOn = false;
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = canvas.alpha;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            canvas.alpha = Mathf.Lerp(alpha, aValue, t);
            yield return null;
        }
    }
}
