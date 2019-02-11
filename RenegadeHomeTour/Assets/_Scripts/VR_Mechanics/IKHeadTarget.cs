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
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var rx = Mathf.Clamp(target.rotation.eulerAngles.x, pitchMin, pitchMax);
        var ry = target.rotation.eulerAngles.y;
        var rz = target.rotation.eulerAngles.z;

        Vector3 newRot = new Vector3(rx, ry, rz);

        transform.position = target.position;
        //transform.localPosition = offset;
    }
}
