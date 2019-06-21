using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public enum state { Walk, Jump, Attack };

    private const float TOL = 0.1f;
    private float MAX_HEALTH = 50f;
    private Animator animator;
    private Material [] mats;

    private Transform myJumpPoint;
    private AudioSource audioSource;
    private FXManager fxManager; 

    public float power = 3f;
    public AudioClip stabSnd, shotSnd, deathSnd, chirp1, chirp2, chirp3, chirp4;
    public ParticleSystem psDissolve, psChunk;
    public Collider[] myCols;
    public Rigidbody rb;

    private float lastJumpTime = 0f;
    [SerializeField]
    private float jumpDelay = 10f;
    [SerializeField]
    private float jumpForce = 100f;

    [SerializeField]
    private float health = 50f;

    public state current_state;
    public float speed = 2f;
    public float jumpDist = 4f;

    public Transform target;
    [SerializeField]
    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        var scale = Random.Range(0.4f, 0.7f);
        health = MAX_HEALTH = scale * 60;

        transform.localScale = new Vector3(scale, scale, scale);
        var renderer = GetComponentInChildren<Renderer>();
        mats = renderer.materials;
        rb = GetComponentInChildren<Rigidbody>();

        current_state = state.Walk;

        animator = GetComponent<Animator>();
        animator.speed = 1f;
        animator.Play("RUN");

        float offset = Random.Range(0f, 2f); // adds variation to the animations
        animator.SetFloat("offset", offset);

        jumpDelay = Random.Range(2f, 15f); // add some variation to aggression

        audioSource = GetComponent<AudioSource>();

        GameManager gm = GameManager.GetInstance();
        gm.OnPlayerDie += OnPlayerDie;
        gm.OnPlayerRespawn += OnPlayerRespawn;

        foreach (Collider pc in gm.playerCols)
            foreach (Collider c in myCols)
            {
                Physics.IgnoreCollision(pc, c);
            }
    }

    void OnCollisionEnter(Collision col)
    {
        if (health > 0 && col.transform.root.CompareTag("Player") && rb.velocity.magnitude > 1f)
        {
            VRMovementController player = col.transform.root.GetComponentInChildren<VRMovementController>();

            if (player != null)
            { 
                player.Hurt(power + rb.velocity.magnitude/2f);
                //Debug.Log("hit for " + (power + rb.velocity.magnitude/2f) + "pts");
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

            fxManager = FXManager.GetInstance();
            fxManager.Burst(FXManager.FX.Dissolve, transform.position, Vector3.zero, 5);
            rb.AddTorque((other.transform.position - transform.position) * dmg);
            audioSource.PlayOneShot(dd.impactSnd);

            health -= dmg;

            fxManager = FXManager.GetInstance();
            fxManager.Burst(FXManager.FX.Dissolve, transform.position, 2);

            rb.AddForce((other.transform.position - transform.position) * (dmg / 100f));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive && health <= 0)
        {
            StopCoroutine("Dissolve");
            StartCoroutine("Dissolve");
            audioSource.PlayOneShot(deathSnd);
            alive = false;
            rb.isKinematic = false;

            animator.SetBool("dead", true);

            fxManager = FXManager.GetInstance();
            fxManager.Burst(FXManager.FX.Dissolve, transform.position, 30);

            GameManager gm = GameManager.GetInstance();
            gm.IncrementKillCount();
        }

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

    void OnPlayerDie()
    {
        target = myJumpPoint;
    }

    void OnPlayerRespawn()
    {
        target = Camera.main.transform;
    }

    IEnumerator Dissolve()
    {
        for (float i = 0f; i < 1f; i += 0.01f)
        {
            for(int m = 0; m <= mats.Length - 1; m ++)
            {
                mats[m].SetFloat("Vector1_69FA3116", i);
            }

            if (i >= 0.8f)
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
                gameObject.SetActive(false);
                StopCoroutine("Dissolve");
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void OnEnable()
    {
        health = MAX_HEALTH;
        alive = true;

        if (mats != null)
        for (int m = 0; m <= mats.Length - 1; m++)
        {
            mats[m].SetFloat("Vector1_69FA3116", 0);
        }
    }
}