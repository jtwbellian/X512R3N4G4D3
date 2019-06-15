using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_gun : VRTool
{
    [SerializeField]
    private bool canFire = true;
    private AudioSource audioSource;
    private Animator anim;
    private Rigidbody currentShot;

    [SerializeField]
    private Light light;
    private float deIntensity;
    private Color deColor;
    [SerializeField]
    private float myIntensity = 2;
    [SerializeField]
    private Color myColor = Color.red;
    public GameObject x;
    private OVRHapticsManager hm;

    public int bulletType;
    public bool fullAuto = false;
    public float cooldown = 1f;
    public float fireSpeed = 10f;
    public Transform gunBarrel;
    public ParticleSystem muzzleFlash;
    public GameObject laserSight;
    
    public override void Init()
    {
        deColor = Color.white;//light.color;
        deIntensity = 0.0125f;//light.intensity;

        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        hm = OVRHapticsManager.instance;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    public override void IndexRelease()
    {
        LightOff();

        if (fullAuto)
            StopCoroutine(FireAuto());

        canFire = true;
    }

    IEnumerator FireAuto()
    {
        while (!canFire)
        {
            Fire();
            yield return new WaitForSeconds(cooldown);
        }
    }

    private void Fire()
    {
        //Rigidbody shot;
        //GameObject GO = Instantiate(x);
        GameObject GO = ObjectPooler.SharedInstance.GetPooledObject((int)bulletType);

        if (GO)
        {
            currentShot = GO.GetComponent<Rigidbody>();
            GO.transform.position = gunBarrel.position;
            GO.transform.rotation = gunBarrel.rotation;

            GO.SetActive(true);
            currentShot.velocity = Vector3.zero;
            currentShot.angularVelocity = Vector3.zero;
            currentShot.AddForce(gunBarrel.forward * fireSpeed + grabInfo.grabbedBy.GetPlayerRB().velocity, ForceMode.VelocityChange);
        }

        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);

        if (anim != null)
            anim.Play("Fire", 0, 0.0f);

        if (hm != null)
            hm.Play(VibrationForce.Hard, GetGrabber().grabbedBy.m_controller, 0.15f);

        if (light != null)
        {
            light.intensity = myIntensity;
            light.color = myColor;
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Clear();
            muzzleFlash.Play();
        }
    }

    public override void IndexTouch()
    {
        canFire = false;

        if (fullAuto)
        {
            StartCoroutine(FireAuto());
        }
        else
        {
            Fire();
            Invoke("LightOff", 0.1f);
        }
        
    }
    /*
    public void Authenicate()
    {
        tutorialGun = false;
    }
    public void MakeTutorial()
    {
        tutorialGun = true;
    }
    */
    public void LightOff()
    {
        if (light)
        {
            light.color = deColor;
            light.intensity = deIntensity;
        }
    }

    public override void ThumbRelease()
    {
        //throw new System.NotImplementedException();
    }

    public override void ThumbTouch()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (laserSight != null && grabInfo.GetGrabber() != null)
            laserSight.SetActive(true);
    }

    public override void OnRelease()
    {
        if (laserSight != null)
            laserSight.SetActive(false);
        base.OnRelease();

        LightOff();
    }
}
