﻿using System.Collections;
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

    public AudioClip stabSnd, shotSnd, deathSnd, chirp1, chirp2, chirp3, chirp4;
    public ParticleSystem psDissolve, psChunk;
    public GameObject beam;
    public Collider[] myCols;

    private float lastBeamTime = 0f;
    [SerializeField]
    private float beamDelay = 25f;
    [SerializeField]
    private float jumpForce = 100f;

    private float health = 500000f;

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
            }
    }

    void OnColliderEnter(Collider other)
    {
        if ((current_state == state.Attack || current_state == state.Jump) && other.transform.root == target)
        {
            VRMovementController player = other.transform.root.GetComponent<VRMovementController>();
            player.Hurt(0.25f);
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

        if (other.transform == target && current_state == state.Walk)
        {
            target = Camera.main.transform.root;
            current_state = state.Seek;
        }


        DoesDammage dd = other.transform.GetComponent<DoesDammage>();

        if (dd != null)
        {
            var dmg = dd.GetDmg();

            if (dmg < TOL)
                return;

            audioSource.PlayOneShot(dd.impactSnd);

            health -= dmg;

            psChunk.transform.position = other.transform.position;
            psChunk.Play();

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
                }

                    break;
            }

            case state.Attack:
            {
                animator.speed = 0.5f;
                animator.SetBool("jump", true);
                beam.SetActive(true);
                break;
            }

            case state.Jump:
            {
                audioSource.PlayOneShot(chirp4);
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