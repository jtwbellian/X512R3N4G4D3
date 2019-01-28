using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPlayerController : MonoBehaviour
{
    public Transform target;
    public float height = 1.5f;
    private float turningRate = 0.001f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != target.position)
        {
            transform.position = new Vector3(target.position.x, target.position.y - height, target.position.z);
        }
        // Turn towards our target rotation.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, turningRate * Time.deltaTime);
    }
}
