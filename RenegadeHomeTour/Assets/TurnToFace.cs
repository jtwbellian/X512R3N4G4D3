using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToFace : MonoBehaviour
{
    Transform target;
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

        if (target.eulerAngles.x < 45)
           transform.rotation = Quaternion.AngleAxis( target.eulerAngles.y, Vector3.up);//Quaternion.Euler(0, target.rotation.eulerAngles.y, 0);
        transform.position = target.position;
    }
}
