using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scr_asteroid : MonoBehaviour
{
    private const float ASTEROID_MASS = 1000f;
    private Rigidbody rb;

    [SerializeField]
    public Vector3 linearForce;
    public Vector3 angularForce;

    // Start is called before the first frame update
    void Start()
    {
        if (linearForce.magnitude + angularForce.magnitude == 0f)
            Randomize();

        Init();
    }

    public void Init()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = angularForce;
        rb.velocity = linearForce;
        rb.drag = 0.001f;
        rb.angularDrag = 0.001f;
        rb.mass = transform.localScale.magnitude * ASTEROID_MASS;
        rb.useGravity = false;
    }

    [ContextMenu("Randomize")]
    void Randomize()
    {
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);
        var z = Random.Range(-1f, 1f);

        linearForce = new Vector3(x, y, z);
        angularForce = new Vector3(z, x, y);
        Init();
    }
}
