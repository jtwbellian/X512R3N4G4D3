using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animator shortAnimator;
    Animator longAnimator;
    bool doorState = false;
    public AudioClip SoundOpen;
    public AudioClip SoundClose;
    public AudioSource SoundSource;

    // Start is called before the first frame update
    void Start()
    {
        shortAnimator = this.transform.Find("ShortDoor").GetComponent<Animator>();
        longAnimator = this.transform.Find("LongDoor").GetComponent<Animator>();
        SoundSource.clip = SoundOpen;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundSource.clip = SoundOpen;
            SoundSource.Play();
            shortAnimator.SetBool("doorOpen", true);
            longAnimator.SetBool("doorOpen", true);
            //If the GameObject is player, output this message in the console
            Debug.Log("Open Sesame!");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundSource.clip = SoundClose;
            SoundSource.Play();
            shortAnimator.SetBool("doorOpen", false);
            longAnimator.SetBool("doorOpen", false);
            //If the GameObject is player, output this message in the console
            Debug.Log("Goodbye.");
        }
    }
}
