using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
	
	public GameObject DoorTrigger;
	public GameObject ShortDoor;
	public GameObject LongDoor;

	Animator ShortAnim;
	Animator LongAnim;


    // Start is called before the first frame update
    void Start()
    {
       	ShortAnim = ShortDoor.GetComponent <Animator > ();
	LongAnim = LongDoor.GetComponent <Animator > ();
    }

	void OnTriggerEnter(Collider coll){
		if(coll.gameObject.tag == "DoorAble"){
		SlideDoors (true);
	}
	}

	void OnTriggerExit(Collider coll){
		if(coll.gameObject.tag == "DoorAble"){
		SlideDoors (false);

	}
	}

	void SlideDoors(bool state){
		ShortAnim.SetBool ("Slide", state);
		LongAnim.SetBool ("Slide", state);
	}
}
