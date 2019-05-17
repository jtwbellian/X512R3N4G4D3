using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrt_gun : VRTool
{
    [SerializeField]
    private bool tutorialGun = false;
    private bool canFire = true;
    private AudioSource audioSource;
    private Animator anim;
    private Light light;
    private OVRHapticsManager hm;

    List<Rigidbody> bulletPool;
    public int poolAmt = 20;

    public Rigidbody bulletType;
    public bool fullAuto = false;
    public float cooldown = 1f;
    public float fireSpeed = 10f;
    public Transform gunBarrel;
    public ParticleSystem muzzleFlash;


    public override void Init()
    {
        light = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        hm = OVRHapticsManager.instance;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();

        bulletPool = new List<Rigidbody>();

        for(int i = 0; i < poolAmt; i++)
        {
            Rigidbody obj = Instantiate(bulletType);
            obj.gameObject.SetActive(false);
            bulletPool.Add(obj);
        }
    }

    public override void IndexRelease()
    {
        if (light != null)
            light.intensity = 0f;

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
        //shot = Instantiate(bulletType);

        for (int i = 0; i < poolAmt; i ++)
        {
            if (!bulletPool[i].gameObject.activeInHierarchy)
            {
                bulletPool[i].transform.position = gunBarrel.position;
                bulletPool[i].transform.rotation = gunBarrel.rotation;
                bulletPool[i].gameObject.SetActive(true);
                bulletPool[i].velocity = transform.forward * fireSpeed + grabInfo.grabbedBy.GetPlayerRB().velocity;
                bulletPool[i].angularVelocity = Vector3.zero;
                break;
            }
        }

        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);

        if (anim != null)
            anim.Play("Fire", 0, 0.0f);


        if (hm != null)
            hm.Play(VibrationForce.Hard, GetGrabber().grabbedBy.m_controller, 0.15f);

        if (light != null)
        {
            light.intensity = .5f;
        }

        if (tutorialGun)
        {
            GameManager.GetInstance().direc.Ping(PING.gunFired);
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
        }
        
    }

    public void Authenicate()
    {
        tutorialGun = false;
    }
    public void MakeTutorial()
    {
        tutorialGun = true;
    }

    public void LightOff()
    {
        light.intensity = 0f;
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
        light.gameObject.SetActive(true);
    }

    public override void OnRelease()
    {
        base.OnRelease();
        light.gameObject.SetActive(false);
    }
}
