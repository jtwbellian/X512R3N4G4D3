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
        if (col.transform.root.gameObject.gameObject.tag == "Player")
        {
            SoundSource.clip = SoundOpen;
            SoundSource.Play();
            shortAnimator.SetBool("Slide", true);
            longAnimator.SetBool("Slide", true);
            //If the GameObject is player, output this message in the console
            Debug.Log("Open Sesame!");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.root.gameObject.tag == "Player")
        {
            SoundSource.clip = SoundClose;
            SoundSource.Play();
            shortAnimator.SetBool("Slide", false);
            longAnimator.SetBool("Slide", false);
            //If the GameObject is player, output this message in the console
            Debug.Log("Goodbye.");
        }
    }
}


/*
 * using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
	
	public GameObject ShortDoor;
	public GameObject LongDoor;

	private Animator ShortAnim;
	private Animator LongAnim;


    // Start is called before the first frame update
    void Start()
    {
       	ShortAnim = ShortDoor.GetComponent <Animator> ();
	    LongAnim = LongDoor.GetComponent <Animator> ();
        ShortAnim.speed = 2;
        LongAnim.speed = 2;

    }

	void OnTriggerEnter(Collider coll){

        if (coll.transform.root.gameObject.CompareTag("Player"))
        {
            SlideDoors(true);
            LongAnim.Play("Long_Open", 1, 0.5f);
            ShortAnim.Play("Short_Open", 1, 0.5f);
        }
    }

	void OnTriggerExit(Collider coll){

		if (coll.gameObject.CompareTag("Player"))
        {
            SlideDoors(false);
	    }
	}

	void SlideDoors(bool State){

        Debug.Log("ShortAnim = " + ShortAnim.ToString());

		ShortAnim.SetBool("Slide", State);
	    LongAnim.SetBool("Slide", State);
	}
}
*/