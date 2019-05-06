using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public enum state { Walk, Jump, Attack };

    private const float TOL = 0.1f;
    private Animator animator;
    private Material [] mats;
    private Rigidbody rb;
    private Transform myJumpPoint;
    private AudioSource audioSource;
    private FXManager fxManager; 

    public float power = 3f;
    public AudioClip stabSnd, shotSnd, deathSnd, chirp1, chirp2, chirp3, chirp4;
    public ParticleSystem psDissolve, psChunk;
    public Collider[] myCols;

    private float lastJumpTime = 0f;
    [SerializeField]
    private float jumpDelay = 10f;
    [SerializeField]
    private float jumpForce = 100f;

    private float health = 50f;

    public state current_state;
    public float speed = 2f;
    public float jumpDist = 4f;

    public Transform target;

    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();
        mats = renderer.materials;
        rb = GetComponent<Rigidbody>();

        current_state = state.Walk;

        animator = GetComponent<Animator>();
        animator.speed = 1f;
        animator.Play("RUN");

        float offset = Random.Range(0f, 2f); // adds variation to the animations
        animator.SetFloat("offset", offset);

        jumpDelay = Random.Range(2f, 15f); // add some variation to aggression

        audioSource = GetComponent<AudioSource>();

        Collider playerCol = Camera.main.transform.root.GetComponentInChildren<CapsuleCollider>();

        if (playerCol != null)
            foreach (Collider c in myCols)
            {
                Physics.IgnoreCollision(playerCol, c);
            }

        fxManager = FXManager.GetInstance();
    }

    void OnCollisionEnter(Collision col)
    {
        if (health > 0 && col.transform.root.CompareTag("Player") && rb.velocity.magnitude > 1f)
        {
            VRMovementController player = col.transform.root.GetComponentInChildren<VRMovementController>();

            if (player != null)
            { 
                player.Hurt(power + rb.velocity.magnitude/2f);
                Debug.Log("hit for " + (power + rb.velocity.magnitude/2f) + "pts");
            }

            return;
        }

        //OnTriggerEnter(other);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("jumpPoint"))
        {
            myJumpPoint = other.transform;
            current_state = state.Jump;
            audioSource.PlayOneShot(chirp3);
            return;
        }

        DoesDammage dd = other.transform.GetComponent<DoesDammage>();

        if (dd != null)
        {
            var dmg = dd.GetDmg();
            //Debug.Log("Hit for " + dmg + " dammage");

            if (dmg < TOL)
                return;

            //psChunk.Play();

            audioSource.PlayOneShot(dd.impactSnd);

            health -= dmg;

            if (alive && health <= 0)
            {
                StartCoroutine("Dissolve");

                audioSource.PlayOneShot(deathSnd);

                alive = false;
                rb.isKinematic = false;
                rb.AddTorque((other.transform.position - transform.position) * dmg);
                animator.SetBool("dead", true);

                //psDissolve.Play();
                fxManager.Burst(2, transform.position, transform.rotation.eulerAngles, 10);

                GameManager gm = GameManager.GetInstance();
                gm.IncrementKillCount();
            }
            else
            {
                fxManager.Burst(2, transform.position, transform.rotation.eulerAngles, 2);
            }

            rb.AddForce((other.transform.position - transform.position) * (dmg / 100f));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (target == null)
            return;

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
                    rb.AddForce(transform.forward * speed ,  ForceMode.Force);

                    if (!target.CompareTag("jumpPoint") && Vector3.Distance(transform.position, target.position) < jumpDist)
                    {
                        current_state = state.Attack;
                        animator.SetBool("jump", true);
                    }

                    break;
            }

            case state.Attack:
            {
                    if (Time.time - lastJumpTime > jumpDelay)
                    {
                        //audioSource.PlayOneShot(chirp1);
                        animator.speed = Random.Range(0.8f,3.0f);
                        animator.SetBool("jump", false);
                        lastJumpTime = Time.time;

                        rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);

                        audioSource.PlayOneShot(chirp2);
                        current_state = state.Walk;
                    }
                    else // back up slowly
                    {
                        Vector3 toTarget = target.position - transform.position;

                        // This constructs a rotation looking in the direction of our target,
                        Quaternion targetRotation = Quaternion.LookRotation(toTarget);

                        // This blends the target rotation in gradually.
                        // Keep sharpness between 0 and 1 - lower values are slower/softer.
                        float sharpness = 0.01f;

                        rb.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, sharpness));
                        rb.AddForce(transform.forward * -0.1f, ForceMode.Force);

                        animator.speed = 1f;
                    }

                break;
            }

            case state.Jump:
                {

                    target = Camera.main.transform;
                    audioSource.PlayOneShot(chirp4);
                    rb.MoveRotation(myJumpPoint.rotation);
                    //rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                    rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);
                    current_state = state.Walk;
                    animator.SetBool("jump", false);
                    break;
                }
        }
    }

    IEnumerator Dissolve()
    {
        for (float i = 0f; i < 1f; i += 0.01f)
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