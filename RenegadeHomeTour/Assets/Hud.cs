using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    private GameManager gm;
    private float lastRefresh;
    private float fadeOutTime = 10f;
    private Rigidbody playerBody;
    private Vector3 lastPos = Vector3.zero;
    private Vector3 lastHudAnchorPos = Vector3.zero;

    public Text[] scoreField;
    public Transform hudAnchor; 

    public float smoothTime = 0.1f;
    public float speed = 25f;
    public Vector3 velocity = Vector3.zero;



    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        playerBody = hudAnchor.transform.root.GetComponentInChildren<VRMovementController>().rigidBody;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastRefresh > fadeOutTime && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime*2f;
        }

        velocity = playerBody.velocity;

        transform.position = SmoothApproach(lastPos, lastHudAnchorPos, hudAnchor.position, speed);
        //Vector3.SmoothDamp(transform.position, HudAnchor.position, ref velocity, smoothTime);
        lastPos = transform.position;
        lastHudAnchorPos = hudAnchor.transform.position;
        transform.LookAt(Camera.main.transform.position);
    }

    Vector3 SmoothApproach(Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float speed)
    {
        float t = Time.deltaTime * speed;
        Vector3 v = (targetPosition - pastTargetPosition) / t;
        Vector3 f = pastPosition - pastTargetPosition + v;
        return targetPosition - v + f * Mathf.Exp(-t);
    }

    public void Refresh()
    {
        foreach (Text t in scoreField)
        {
            t.text = gm.GetCrabsKilled().ToString();
        }
        canvasGroup.alpha = 1;
        lastRefresh = Time.time;
    }
}
