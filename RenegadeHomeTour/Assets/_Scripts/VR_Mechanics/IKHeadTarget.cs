using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHeadTarget : MonoBehaviour
{
    public Transform target;
    [Range(-180f, 180f)]
    public float pitchMax;
    [Range(-180f, 180f)]
    public float pitchMin;
    public float yoffset = 0.0f;
    public float zoffset = 0.0f;
    public IKPlayerController ik;


    // Start is called before the first frame update
    void Start()
    {
        target = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused)
            return;
        //Debug.Log("x angle= " + target.eulerAngles.x);

        if (target.eulerAngles.x < 45 || target.eulerAngles.x > 180)
           transform.rotation = Quaternion.AngleAxis(target.eulerAngles.y, Vector3.up);

        transform.position = (target.position + transform.forward * zoffset + transform.up * ((-1 * ik.height) + yoffset));
    }
}
