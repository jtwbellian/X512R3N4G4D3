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
    public AudioClip stabSnd, shotSnd, deathSnd;

    private float lastJumpTime = 0f;
    [SerializeField]
    private float jumpDelay = 10f;
    [SerializeField]
    private float jumpForce = 100f;

    private float numAttacks = 3;
    private float attack = 0;
    private float health = 100f;

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

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("jumpPoint"))
        {
            myJumpPoint = col.transform;
            current_state = state.Jump;
            return;
        }

        DoesDammage dd = col.transform.GetComponent<DoesDammage>();
        
        if (dd != null)
        {
            var dmg = dd.GetDmg();

            if (dmg < TOL)
                return;

            audioSource.PlayOneShot(dd.impactSnd);

            health -= dmg;

            if (alive && health <= 0)
            {
                StartCoroutine("Dissolve");

                audioSource.PlayOneShot(deathSnd);

                alive = false;
                rb.isKinematic = false;
                rb.AddTorque(col.transform.position - transform.position);
                animator.SetBool("dead", true);

                GameManager gm = GameManager.GetInstance();
                gm.IncrementKillCount();
            }
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
                    transform.LookAt(target);
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
                        animator.speed = Random.Range(0.8f,3.0f);
                        animator.SetBool("jump", true);
                        lastJumpTime = Time.time;
                        rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);

                        if (attack > numAttacks)
                        {
                            attack = 0;
                            current_state = state.Walk;
                        }
                        else
                            attack++;
                    }
                    else // back up slowly
                    {
                        transform.LookAt(target);
                        animator.speed = 1f;
                        rb.AddForce(transform.forward * -0.1f, ForceMode.Force);
                    }

                break;
            }

            case state.Jump:
                {

                    target = Camera.main.transform;

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
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
