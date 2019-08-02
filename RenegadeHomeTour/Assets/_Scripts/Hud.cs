using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Icon
{
    analogFwd, analogClick, grab, holster, calibrate, use, advert, NONE = -1
}

public class Hud : EVActor
{
    private CanvasGroup canvasGroup;
    private GameManager gm;
    private float lastRefresh;
    private float fadeOutTime = 10f;
    private Rigidbody playerBody;
    private Vector3 lastPos = Vector3.zero;
    private Vector3 lastHudAnchorPos = Vector3.zero;

    public GameObject menu;
    public Text[] messageField;

    public string message;
    public Text[] scoreField;
    public Text[] scoreTitle;
    public Transform hudAnchor; 
    public Animator iconAnimator;

    public float smoothTime = 0.1f;
    public float speed = 5f;
    public Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        subscribesTo = AppliesTo.PLAYER;
        gm = GameManager.GetInstance();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        playerBody = hudAnchor.transform.root.GetComponentInChildren<VRMovementController>().rigidBody;
        iconAnimator.SetInteger("index", -1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canvasGroup.alpha = Mathf.Clamp(1f - Vector3.Distance(transform.position, hudAnchor.position)*2.5f, 0f, 1f);
        /*
        if (!menu.activeSelf && Time.time - lastRefresh > fadeOutTime && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 1.2f;
        }
        if (menu.activeSelf)
        {
            canvasGroup.alpha = 1f;
        }*/
        //velocity = playerBody.velocity;

        transform.position = SmoothApproach(lastPos, lastHudAnchorPos, hudAnchor.position, speed + playerBody.velocity.magnitude);

        //speed = 20f + playerBody.velocity.magnitude;

        lastPos = transform.position;
        lastHudAnchorPos = hudAnchor.transform.position;
        transform.LookAt(Camera.main.transform.position);
    }

    public void ToggleMenu()
    {
        //if (menu.active)
        //{
        //    menu.SetActive(false);
        //}
        //else
        //{
        if (!menu.activeSelf)
            menu.SetActive(true);
        //}
    }

    Vector3 SmoothApproach(Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float speed)
    {
        float t = Time.deltaTime * (speed + playerBody.velocity.magnitude);
        Vector3 v = (targetPosition - pastTargetPosition) / t;
        Vector3 f = pastPosition - pastTargetPosition + v;
        return targetPosition - v + f * Mathf.Exp(-t);
    }
        
    public void ShowImage(Icon icon, float time)
    {
        //iconAnimator.transform.GetComponent<SpriteRenderer>().color = new Vector4(0.9f, 0.9f, 0.9f, 1f);
        iconAnimator.SetInteger("index",(int) icon);
        ShowSubtitles();

        Invoke("HideImage", time);

        Refresh();
    }

    public void ShowImage(Icon icon)
    {
        iconAnimator.transform.GetComponent<SpriteRenderer>().enabled = true;
        ShowSubtitles();
  
        switch (icon)
        {
            case Icon.analogClick: message = "Click Both Joysticks to return LuxPlate"; break;
            case Icon.analogFwd: message = "Use the Left Joystick to Jet Boost"; break;
            case Icon.grab: message = "Squeeze Grab Button to Grab objects"; break;
            case Icon.holster: message = "Drop Weapons onto Orbs to Holster"; break;
            case Icon.use: message = "Pull the Index Trigger to Shoot"; break;
            default: message = "" ; break;
        }

        //iconAnimator.transform.GetComponent<SpriteRenderer>().color = new Vector4(0.9f, 0.9f, 0.9f, 0.8f);
        iconAnimator.SetInteger("index", (int)icon);
        Refresh();
    }

    public void HideImage()
    {
        HideSubtitles();
        iconAnimator.SetInteger("index", -1);
        //iconAnimator.transform.GetComponent<SpriteRenderer>().color = new Vector4(0.9f, 0.9f, 0.9f, 0f);
    }

    public void HideSubtitles()
    {
        foreach (Text t in messageField)
        {
            t.gameObject.SetActive(false);
        }

    }

    public void ShowSubtitles()
    {
        foreach (Text t in messageField)
        {
            t.gameObject.SetActive(true);
            t.text = message;
        }

    }

    public void SetMessage(string str)
    {
        message = str;
        ShowSubtitles();
        CancelInvoke("HideSubtitles");
        Invoke("HideSubtitles", 30f);
    }

    public void ShowKills()
    {
        foreach(Text s in scoreField)
        {
            s.gameObject.SetActive(true);
        }
        
        foreach(Text s in scoreTitle)
        {
            s.gameObject.SetActive(true);
        }

    }

    public void Refresh()
    {
        gm = GameManager.GetInstance();

        foreach (Text t in scoreField)
        {
            t.text = gm.GetCrabsKilled().ToString();
        }

        foreach (Text t in messageField)
        {
            t.text = message;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 1;

        lastRefresh = Time.time;
    }

    public override void BeginEvent()
    {
        //Icon iconType = (Icon)System.Enum.Parse(typeof(Icon), myEvent.myName);
        Icon iconType = Icon.NONE;

        if (System.Enum.TryParse<Icon>(myEvent.myName, out iconType))
        {
            ShowImage(iconType);
            CompleteEvent();
        }
        else
        if (iconType == Icon.NONE)
        {
            HideImage();
            CompleteEvent();
        }

    }
}
