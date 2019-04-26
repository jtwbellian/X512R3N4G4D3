using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inspectRay : MonoBehaviour {

    private GameObject lastObj = null;
    private ToonLines lastLine;
    private scr_inspectorLabel lastLabel;
    private bool canCheck = true;
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        int layerMask = 1 << 9;

        bool rayBlocked = Physics.Raycast(transform.position, (transform.rotation * Vector3.forward), out hit, Mathf.Infinity, layerMask);

        // Does the ray intersect any objects excluding the player layer
        if (rayBlocked && canCheck)//Physics.Raycast(transform.position, (transform.rotation * Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            // was pointing at a different object
            if (lastObj)
            {
                lastLine.TurnOff();
                lastLabel.FadeOut();
            }

            Debug.DrawRay(transform.position, (transform.rotation * Vector3.forward) * hit.distance, Color.green);

            ToonLines line = hit.transform.GetComponent<ToonLines>();
            scr_inspectorLabel label = hit.transform.GetComponentInChildren<scr_inspectorLabel>();

            if (line == null || label == null)
                return;

            line.TurnOn();
            label.FadeIn();

            lastObj = hit.transform.gameObject;
            lastLine = line;
            lastLabel = label;

        }
    }
}
