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
        //ShortAnim.updateMode.UnscaledTime;
        //LongAnim.updateMode.UnscaledTime;
        Debug.Log((LongAnim == null) ? "Long anim is NULL" : "We good");
        Debug.Log((ShortAnim == null) ? "Short anim is NULL" : "We good");
    }

	void OnTriggerEnter(Collider coll){

        Debug.Log("Trigger entered");
        //if (coll.gameObject.tag == "Player")
        if (coll.gameObject.CompareTag("Player"))
        {
            var State = true;
            SlideDoors(State);
            LongAnim.Play("Long_Open", 1, 0.5f);
            ShortAnim.Play("Short_Open", 1, 0.5f);
        }
    }

	void OnTriggerExit(Collider coll){

		if (coll.gameObject.CompareTag("Player"))
        {
            var State = false;
            SlideDoors(State);
	    }
	}

	void SlideDoors(bool State){

        Debug.Log("ShortAnim = " + ShortAnim.ToString());

		ShortAnim.SetBool("Slide", State);
	    LongAnim.SetBool("Slide", State);
	}
}
