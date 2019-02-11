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
           // var state = true;
            SlideDoors(state);
            LongAnim.Play("Long_Open", 0, 1f);
            ShortAnim.Play("Short_Open", 0, 1f);
        }
    }

	void OnTriggerExit(Collider coll){

		if (coll.gameObject.CompareTag("Player"))
        {
           // var state = false;
            //SlideDoors(state);
	    }
	}

//	void SlideDoors(bool state){

//        Debug.Log("ShortAnim = " + ShortAnim.ToString());

		//ShortAnim.SetBool("Slide", state);
		//LongAnim.SetBool("Slide", state);
//	}
}
