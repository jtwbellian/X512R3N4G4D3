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
    public AudioClip stabSnd, shotSnd, deathSnd, chirp1, chirp2, chirp3, chirp4;
    public ParticleSystem psDissolve, psChunk;
    public Collider[] myCols;

    private float lastJumpTime = 0f;
    [SerializeField]
    private float jumpDelay = 10f;
    [SerializeField]
    private float jumpForce = 100f;

    private float numAttacks = 2;
    private float attack = 0;
    private float health = 1000f;

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
    }

    void OnColliderEnter(Collider other)
    {
        if (other.transform.root == target)
        {
            VRMovementController player = other.transform.root.GetComponent<VRMovementController>();
            var dmg = rb.velocity.magnitude * Time.deltaTime * 10f;
            player.Hurt(dmg);
            return;
        }

        OnTriggerEnter(other);
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
                psDissolve.Play();
                GameManager gm = GameManager.GetInstance();
                gm.IncrementKillCount();
            }

            rb.AddForce((other.transform.position - transform.position) * (dmg / 100f));
        }

    }


    // Update is called once per frame
    void Update()
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
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, sharpness);


                    rb.AddForce(transform.forward * speed ,  ForceMode.Force);

                    if (!target.CompareTag("jumpPoint") && Vector3.Distance(transform.position, target.position) < jumpDist)
                    {
                        current_state = state.Attack;
                    }

                    break;
            }

            case state.Attack:
            {
                    if (Time.time - lastJumpTime > jumpDelay)
                    {
                        //audioSource.PlayOneShot(chirp1);
                        animator.speed = Random.Range(0.8f,3.0f);
                        animator.SetBool("jump", true);
                        lastJumpTime = Time.time;
                        rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);

                        if (attack > numAttacks)
                        {
                            audioSource.PlayOneShot(chirp2);
                            attack = 0;
                            current_state = state.Walk;
                        }
                        else
                            attack++;
                    }
                    else // back up slowly
                    {

                        Vector3 toTarget = target.position - transform.position;

                        // This constructs a rotation looking in the direction of our target,
                        Quaternion targetRotation = Quaternion.LookRotation(toTarget);

                        // This blends the target rotation in gradually.
                        // Keep sharpness between 0 and 1 - lower values are slower/softer.
                        float sharpness = 0.01f;
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, sharpness);

                        animator.speed = 1f;
                        rb.AddForce(transform.forward * -0.1f, ForceMode.Force);
                    }

                break;
            }

            case state.Jump:
                {

                    target = Camera.main.transform;
                    audioSource.PlayOneShot(chirp4);
                    transform.rotation = myJumpPoint.rotation;
                    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                    rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);
                    current_state = state.Walk;
                     
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
