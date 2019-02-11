using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetoid : MonoBehaviour
{
    public int numMoons;
    public float planetScale;
    public float atmoScale;
    public float rotSpeed;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = new Vector3(rotSpeed,0);
        rb.angularDrag = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
