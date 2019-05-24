using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaCrabController : MonoBehaviour
{
    public enum state { Seek, Walk, Jump, Attack, None };

    private const float TOL = 0.1f;
    private Animator animator;
    private Material [] mats;
    private Rigidbody rb;
    private Transform myJumpPoint;
    private AudioSource audioSource;
    private FXManager fxManager;

    public AudioClip stabSnd, shotSnd, deathSnd, chirp1, chirp2, chirp3, chirp4;
    public ParticleSystem psDissolve, psChunk;
    public GameObject beam;
    public Collider[] myCols;

    public Collider[] ignoreColliders;

    private float lastBeamTime = 0f;
    private float chargeDelay = 20f;
    [SerializeField]
    private float beamDelay = 800f;
    private Material beamMat; 
    [SerializeField]
    private float jumpForce = 100f;

    private float health = 500000f;

    public state current_state;
    public float speed = 2f;
    public float jumpDist = 4f;
    private float beamStrength = 2f;

    public Transform target;

    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {

        var renderer = GetComponentInChildren<Renderer>();
        mats = renderer.materials;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        current_state = state.Jump;

        animator = GetComponent<Animator>();
        animator.speed = 1f;
        animator.Play("RUN");
        animator.SetBool("jump", true);

        float offset = Random.Range(0f, 2f); // adds variation to the animations
        animator.SetFloat("offset", offset);

        beamDelay = Random.Range(2f, 15f); // add some variation to aggression

        audioSource = GetComponent<AudioSource>();

        Collider playerCol = Camera.main.transform.root.GetComponentInChildren<CapsuleCollider>();

        if (playerCol != null)
            foreach (Collider c in myCols)
            {
                Physics.IgnoreCollision(playerCol, c);

                foreach (Collider ic in ignoreColliders)
                {
                    Physics.IgnoreCollision(c, ic);
                }
            }

        var beamRenderer = beam.GetComponentInChildren<MeshRenderer>();
        beamMat = beamRenderer.material;

        fxManager = FXManager.GetInstance();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.transform == target && current_state == state.Walk)
        {
            target = Camera.main.transform;
            current_state = state.Seek;
            audioSource.PlayOneShot(chirp2);
        }

        DoesDammage dd = other.transform.GetComponent<DoesDammage>();

        if (dd != null)
        {
            var dmg = dd.GetDmg();

            if (dmg < TOL)
                return;

            audioSource.PlayOneShot(dd.impactSnd);

            fxManager = FXManager.GetInstance();
            fxManager.Burst(FXManager.FX.Chunk, other.transform.position, 2);
            fxManager.Burst(FXManager.FX.Dissolve, other.transform.position, 10);

            health -= dmg;

            if (alive && health <= 0)
            {
                StartCoroutine("Dissolve");

                audioSource.PlayOneShot(deathSnd);

                alive = false;
                rb.isKinematic = false;
                rb.AddTorque((other.transform.position - transform.position) * dmg);
                animator.SetBool("dead", true);
                psDissolve.Play();
                GameManager gm = GameManager.GetInstance();
                gm.IncrementKillCount();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (current_state == state.Attack && other.transform.root.CompareTag("Player"))
        {
            VRMovementController player = other.transform.root.GetComponent<VRMovementController>();
            player.Hurt(beamStrength);
            FXManager.GetInstance().Burst(FXManager.FX.Beam, other.transform.position, 15);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (current_state)
        {
            case state.Walk:
                {
                    animator.speed = 1.5f;

                    Vector3 toTarget = target.position - transform.position;

                    // This constructs a rotation looking in the direction of our target,
                    Quaternion targetRotation = Quaternion.LookRotation(toTarget);

                    // This blends the target rotation in gradually.
                    // Keep sharpness between 0 and 1 - lower values are slower/softer.
                    float sharpness = 0.1f;

                    rb.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, sharpness));
                    rb.AddForce(transform.forward * speed, ForceMode.Force);
                    break;
                }

            case state.Seek:
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                animator.speed = 0.2f;

                Vector3 toTarget = target.position - transform.position;

                // This constructs a rotation looking in the direction of our target,
                Quaternion targetRotation = Quaternion.LookRotation(toTarget);

                // This blends the target rotation in gradually.
                // Keep sharpness between 0 and 1 - lower values are slower/softer.
                float sharpness = 0.025f;

                rb.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, sharpness));

                if (Time.time - lastBeamTime > beamDelay)
                {
                    lastBeamTime = Time.time;
                    current_state = state.Attack;
                    audioSource.PlayOneShot(chirp4);
                    beam.SetActive(true);
                    animator.speed = 0.1f;
                    animator.SetBool("jump", true);
                 }

                 break;
            }

            case state.Attack:
            {
                    if (Time.time - lastBeamTime < chargeDelay)
                    {
                        //Debug.Log("Time: " + Time.time + ", last beam: " + lastBeamTime);
                        var amt = Mathf.Lerp(2f, 0.1f, Mathf.Abs(0.5f - (Time.time - lastBeamTime) / chargeDelay)*2f);

                        if (amt < 1f)
                        { 
                            Vector3 toTarget = target.position - transform.position;

                            // This constructs a rotation looking in the direction of our target,
                            Quaternion targetRotation = Quaternion.LookRotation(toTarget);

                            // This blends the target rotation in gradually.
                            // Keep sharpness between 0 and 1 - lower values are slower/softer.
                            float sharpness = 1 - amt;

                            rb.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, sharpness));
                        }

                        int layerMask = 1 << 2;
                        layerMask = ~layerMask;

                        RaycastHit hit;
                        // Does the ray intersect any objects excluding the player layer
                        if (Physics.Raycast(beam.transform.position, beam.transform.TransformDirection(Vector3.forward), out hit, 10f, layerMask))
                        {
                            FXManager.GetInstance().Burst(FXManager.FX.Beam, hit.point, 3);
                            var beamObj = beam.GetComponentInChildren<MeshRenderer>().transform;
                            beamObj.localScale.Set(beamObj.localScale.x, Vector3.Distance(hit.point, beamObj.position), beamObj.localScale.z);
                            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                        }

                        beamStrength = Mathf.Lerp(0.1f, 1f, amt);
                        beamMat.SetFloat("_strength", beamStrength);

                    }
                    else
                    {
                        beamStrength = 0f;
                        beam.SetActive(false);
                        lastBeamTime = Time.time;
                        beamMat.SetFloat("_strength", 0.1f);
                        current_state = state.Seek;
                        audioSource.PlayOneShot(chirp2);
                    }
                break;
            }

            case state.Jump:
            {
                audioSource.PlayOneShot(chirp1);
                rb.MoveRotation(Quaternion.Euler(transform.position - target.position).normalized);
                rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);
                current_state = state.Walk;
                break;
            }
        }
    }

    IEnumerator Dissolve()
    {
        for (float i = 0f; i < 1f; i += 0.002f)
        {
            for(int m = 0; m <= mats.Length - 1; m ++)
            {
                mats[m].SetFloat("Vector1_69FA3116", i);
            }

            if (i > 0.8f)
            {
                // Remove hats before destroying self
                GrabMagnet mag = GetComponentInChildren<GrabMagnet>();
                 
                if (!mag.empty)
                {
                    Transform tool = mag.transform.GetChild(0);

                    if (tool != null)
                    {
                        tool.parent = null;
                    }
                }

                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}