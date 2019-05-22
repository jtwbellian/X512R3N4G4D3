using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VRMovementController : MonoBehaviour
{
    private const float TOL = 0.05f;
    private const float MAX_VIGNETTE = 0.85f;
    private const float MIN_VIGNETTE = 0.3f;
    private const float MAX_SPEED = 25f;
    private float boostbar_width = 0.0f;
    public Rigidbody rigidBody;
    private Transform head;
    private float rechargeRate = 2f;
    private bool ReadyToSnapTurn = false;
    private Vignette vignette;
    private ColorGrading colorGrading;
    private float boostRate = 0.7f;
    [SerializeField]
    private float shields = 100f;
    private AudioSource audio;

    private OVRGrabber[] grabbers;

    public IKPlayerController ikController;

    public Color c_warm;
    public Color c_cool; 

    public bool canBoost = false;
    public float speed = 2f;
    public float boost = 1f;

    public AudioClip [] hurtClips;

    //public float rotationRatchet = 45f;

    public Text velocityLabel;
    public Transform boostBar;
    public PostProcessVolume ppVolume;
    private GameManager gm;

    private Vector3 euler;
    private Vector3 lastPos; 

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        
        if (boostBar == null)
            Debug.Log("ERROR: Boost bar is null, dummy!");
        else
            boostbar_width = boostBar.localScale.x;
            
        grabbers = transform.root.GetComponentsInChildren<OVRGrabber>();
        
        ppVolume.profile.TryGetSettings(out vignette);
        ppVolume.profile.TryGetSettings(out colorGrading);
        //body = GetComponentInChildren<IKPlayerController>().transform;
        //rb = body.GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        rigidBody.maxDepenetrationVelocity = 0.5f;
        head = Camera.main.transform; //GetComponentInChildren<Camera>().transform;
        audio = GetComponent<AudioSource>();
        ikController = GetComponentInChildren<IKPlayerController>();


    }

    public void ViewRatchet(float amt)
    {
        euler = transform.rotation.eulerAngles;
        lastPos = head.position;

        euler.y += amt;
        transform.rotation = Quaternion.Euler(euler);
        transform.position += lastPos - head.position;
        ReadyToSnapTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Die
        if (shields <= 0 && !gm.playerIsDead)
        {
            canBoost = false;
            gm.PlayerDie();
            return;
        }

        if (canBoost)
        {
            // Use Boost Jets
            var stickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
            var stickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;

            if ((Mathf.Abs(stickY) > 0f || Mathf.Abs(stickX) > 0f) && boost > 0f)//rigidBody.velocity.magnitude < MAX_SPEED)
            {
                rigidBody.AddForce(head.forward * (speed * stickY * (boost + 0.1f)) + 
                                    head.right * (speed * stickX * (boost + 0.1f)), ForceMode.Force);

                if (!audio.isPlaying)
                {
                    audio.Play();
                }

                audio.volume = Mathf.Clamp(Mathf.Abs(stickY) + Mathf.Abs(stickX), 0, 1);
                audio.pitch = boost;

                boost -= Time.deltaTime * (Mathf.Abs(stickY) + Mathf.Abs(stickX)) * 0.25f;
            }
            else if (boost < 1f)
            {
                boost += Time.deltaTime * boostRate;
                audio.volume = 0f;
                audio.Stop();
            }

            boostBar.localScale = new Vector3(boostbar_width * boost, boostBar.localScale.y, boostBar.localScale.z);
        }


        // Turn View Left
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            if (ReadyToSnapTurn)
            {
                ViewRatchet(-45f);
            }
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            if (ReadyToSnapTurn)
            {
                ViewRatchet(45f);
            }
        }
        else if (ReadyToSnapTurn == false)
        {
            ReadyToSnapTurn = true;
        }


        // Show dammage "heat"
        if (shields < 100f)
        { 
            if (shields > 0f)
                shields += Time.deltaTime * rechargeRate;
            colorGrading.saturation.value = Mathf.Lerp( -100f, 25, shields/100f);
        }

        var canVig = true;

        if (grabbers[0].grabbedObject is OVRClimbable || grabbers[1].grabbedObject is OVRClimbable)
            canVig = false;

        if (vignette != null )
        {
            if (canVig)
                vignette.intensity.value = Mathf.Clamp(Mathf.Abs(rigidBody.velocity.magnitude) / 2f, MIN_VIGNETTE, MAX_VIGNETTE);
            else
                vignette.intensity.value = MAX_VIGNETTE / 2f;
        }
    }

    public void AllowBoost()
    {
        canBoost = true;
        boostBar.parent.gameObject.SetActive(true);
    }
        
    public void Hurt(float amt)
    {
        int ranSound = Random.Range(0, hurtClips.Length - 1);

        gm.sm.player.PlayOneShot(hurtClips[ranSound]);

        if (shields > 0)
            shields -= amt;
    }
}