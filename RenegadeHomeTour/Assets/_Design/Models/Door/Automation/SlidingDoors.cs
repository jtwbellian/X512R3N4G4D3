using UnityEngine;

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
