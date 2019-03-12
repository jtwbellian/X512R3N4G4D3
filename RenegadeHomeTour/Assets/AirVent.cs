using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    Animator anim;
    AudioSource audio;
    public AudioClip flapSnd;
    public AudioClip rattleStartSnd;

    private bool rattle = false;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        startPos = transform.position;

        if (anim == null)
        {
            Debug.Log("Animator not found!");
        }
    }

    public void StartShaking()
    {
        var sm = SoundManager.GetInstance();
        sm.environment.clip = rattleStartSnd;

        if (!sm.environment.isPlaying)
            sm.environment.Play();

        rattle = true;
    }

    public void StopShaking()
    {
        rattle = false;
        transform.position = startPos;
    }

    public void Reset()
    {
        anim.SetBool("Flap", false);
    }


    void Update()
    {
        if (rattle)
        {
            float shake = Mathf.Sin(Time.time * 500f) * 0.01f;

            transform.position = startPos + new Vector3(shake, -shake, shake );
        }
    }

    public void Flap()
    {
        if (audio != null)
            audio.Play();

        if (anim == null)
            anim = GetComponent<Animator>();

        anim.speed = 1f;
        anim.SetBool("Flap", true);
        Invoke("Reset", 2f);
    }
}
